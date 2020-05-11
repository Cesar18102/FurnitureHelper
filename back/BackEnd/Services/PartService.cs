﻿using System;
using System.Linq;
using System.Collections.Generic;

using Autofac;

using Models;
using DataAccessHolder;
using DataAccessContract;

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

        public PartStore GetOwned(SessionDto ownerSession)
        {
            IEnumerable<ConcretePartModel> concreteParts = GetOwnedConcrete(ownerSession);
            return new PartStore(concreteParts);
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
            PartStore store = new PartStore();
            foreach (PartModel part in PartRepo.GetAll())
            {
                int stored = PartRepo.GetCountOfStored(part.Id);
                PartStorePosition position = new PartStorePosition(part, stored);
                store.Positions.Add(position);
            }
            return store;
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

                if (partDto.ControllerMac != null && ConcretePartRepo.GetPartByMac(partDto.ControllerMac) != null)
                    throw new ConflictException("mac address");

                if (partDto.ControllerMac != null)
                {
                    ConcretePartModel model = Mapper.Map<AddConcretePartDto, ConcretePartModel>(partDto);
                    return ConcretePartRepo.Create(model);
                }
                else if (part.ConnectionHelpers.Count() == 0)
                {
                    ICollection<ConcretePartModel> created = new List<ConcretePartModel>();
                    ConcretePartModel model = Mapper.Map<AddConcretePartDto, ConcretePartModel>(partDto);

                    for (int i = 0; i < partDto.Amount.GetValueOrDefault(); ++i)
                        created.Add(ConcretePartRepo.Create(model));

                    return created.LastOrDefault();
                }
                else
                    throw new NotFoundException("controller mac");

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
            //pin service
            List<int> usedPins = part.ConnectionHelpers.Aggregate(
                new List<int>(),
                (acc, helper) => acc.Append(helper.IndicatorPinNumber).Append(helper.ReaderPinNumber).ToList()
            );

            if (usedPins.Distinct().Count() != usedPins.Count())
                throw new ConflictException("pin");
        }
    }
}
