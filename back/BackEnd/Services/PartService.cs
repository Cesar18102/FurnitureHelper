using System;
using System.Linq;
using System.Collections.Generic;

using Autofac;

using Models;
using DataAccessHolder;
using DataAccessContract;

using Services.Pin;
using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace Services
{
    public class PartService : ServiceBase, IPartService
    {
        private static readonly IConcretePartRepo ConcretePartRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IConcretePartRepo>();
        private static readonly IPartRepo PartRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IPartRepo>();

        private static readonly SessionService SessionService = ServiceDependencyHolder.ServicesDependencies.Resolve<SessionService>();
        private static readonly AdminService AdminService = ServiceDependencyHolder.ServicesDependencies.Resolve<AdminService>();
        private static readonly PinService PinService = ServiceDependencyHolder.ServicesDependencies.Resolve<PinService>();

        private static ITradeService tradeService = null;
        private ITradeService TradeService
        {
            get
            {
                if (tradeService == null)
                    tradeService = ServiceDependencyHolder.ServicesDependencies.Resolve<ITradeService>();
                return tradeService;
            }
        } 

        public PartStore GetOwned(SessionDto ownerSession)
        {
            SessionService.CheckSession(ownerSession);
            return new PartStore(GetOwnedConcrete(ownerSession));
        }

        public InvariantPartStore GetOwnedInvariant(SessionDto ownerSession)
        {
            SessionService.CheckSession(ownerSession);
            return new InvariantPartStore(GetOwnedConcrete(ownerSession));
        }

        public PartStore GetUserBids()
        {
            IEnumerable<ConcretePartModel> forSellParts = ConcretePartRepo.GetForSellParts();
            return new PartStore(forSellParts);
        }

        public IEnumerable<ConcretePartModel> GetOwnedConcrete(SessionDto ownerSession)
        {
            return ProtectedExecute<SessionDto, ConcretePartModel>(sessionDto =>
            {
                SessionService.CheckSession(sessionDto);
                return ConcretePartRepo.GetOwnedByUser(sessionDto.UserId.GetValueOrDefault());
            }, ownerSession);
        }

        public PartStore GetStore()
        {
            List<ConcretePartModel> stored = ConcretePartRepo.GetStored().ToList();
            List<ConcretePartModel> pending = TradeService.GetPendingPartsList().ToList();
            IEnumerable<ConcretePartModel> store = stored.Where(part => pending.FirstOrDefault(p => p.Id == part.Id) == null).ToList();

            return new PartStore(PartRepo.GetAll(), store);
        }

        public ConcretePartModel RegisterConcretePart(AddConcretePartDto dto)
        {
            return ProtectedExecute<AddConcretePartDto, ConcretePartModel>(partDto =>
            {
                AdminService.CheckActiveAdmin(partDto.AdminSession);

                PartModel part = PartRepo.Get(partDto.PartId.GetValueOrDefault());
                if (part == null)
                    throw new NotFoundException("part");

                MaterialModel material = part.PossibleMaterials.FirstOrDefault(mat => mat.Id == partDto.MaterialId);
                if (material == null)
                    throw new NotFoundException("possible material");

                PartColorModel color = material.PossibleColors.FirstOrDefault(col => col.Id == partDto.ColorId);
                if (color == null)
                    throw new NotFoundException("possible color");

                if (partDto.ControllerMac == null && partDto.Amount == null)
                    throw new NotFoundException("nor controller_mac neither amount");

                if(partDto.ControllerMac == null && part.ConnectionHelpers.Count() != 0)
                    throw new NotFoundException("controller mac for assigned connection helpers");
                
                if (partDto.ControllerMac != null && part.ConnectionHelpers.Count() == 0)
                    throw new NotFoundException("connection helpers to embed controller");

                if (partDto.ControllerMac != null && ConcretePartRepo.GetPartByMac(partDto.ControllerMac) != null)
                    throw new ConflictException("mac address");

                if (partDto.ControllerMac == null)
                {
                    ICollection<ConcretePartModel> created = new List<ConcretePartModel>();
                    ConcretePartModel model = Mapper.Map<AddConcretePartDto, ConcretePartModel>(partDto);

                    for (int i = 0; i < partDto.Amount.GetValueOrDefault(); ++i)
                        created.Add(ConcretePartRepo.Create(model));

                    return created.LastOrDefault();
                }

                return ConcretePartRepo.Create(Mapper.Map<AddConcretePartDto, ConcretePartModel>(partDto));
            }, dto);
        }

        public PartModel RegisterPart(AddPartDto dto)
        {
            return ProtectedExecute<AddPartDto, PartModel>(partDto =>
            {
                AdminService.CheckActiveSuperAdmin(partDto.SuperAdminSession);
                PartModel model = Mapper.Map<AddPartDto, PartModel>(partDto);
                CheckPinConflict(model);
                return PartRepo.Create(model);
            }, dto);
        }

        public PartModel UpdatePart(UpdatePartDto dto)
        {
            return ProtectedExecute<UpdatePartDto, PartModel>(partDto =>
            {
                AdminService.CheckActiveSuperAdmin(partDto.SuperAdminSession);
                PartModel model = Mapper.Map<UpdatePartDto, PartModel>(partDto);
                CheckPinConflict(model);
                return PartRepo.Update(model.Id, model);
            }, dto);
        }

        private void CheckPinConflict(PartModel part)
        {
            if (part.ConnectionHelpers == null)
                return;

            List<int> usedPins = part.ConnectionHelpers.Aggregate(new List<int>(), (acc, helper) => 
                acc.Append(helper.IndicatorPinNumber).Append(helper.ReaderPinNumber).Append(helper.ReaderPinNumberOther).ToList()
            );

            if (!usedPins.TrueForAll(PinService.IsValidConnectionHelperPin))
                throw new ArgumentException("pin number is invalid");

            if (usedPins.Distinct().Count() != usedPins.Count())
                throw new ConflictException("pin");
        }

        public PartModel Get(int partId)
        {
            return PartRepo.Get(partId);
        }

        public ControllerConfigModel GetControllerConfig(ControllerPingDto pingDto)
        {
            ConcretePartModel part = ConcretePartRepo.GetPartByMac(pingDto.Mac);

            if (part == null)
                throw new NotFoundException("concrete part with specified mac");

            List<int> indicators = part.Part.ConnectionHelpers.Select(helper => helper.IndicatorPinNumber).ToList();
            List<int> readers = part.Part.ConnectionHelpers.Aggregate(
                new List<int>(), 
                (acc, helper) => acc.Append(helper.ReaderPinNumber).Append(helper.ReaderPinNumberOther).ToList()
            ).ToList();

            return new ControllerConfigModel(indicators, readers);
        }
    }
}
