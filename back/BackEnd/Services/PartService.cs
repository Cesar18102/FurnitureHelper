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
        private static readonly AdminService AdminService = ServiceDependencyHolder.ServicesDependencies.Resolve<AdminService>();

        public ConcretePartModel RegisterConcretePart(AddConcretePartDto dto)
        {
            AdminService.CheckActiveAdmin(dto.AdminSession);

            PartModel part = PartRepo.Get(dto.PartId.GetValueOrDefault());
            if (part == null)
                throw new NotFoundException("part");

            MaterialModel material = part.PossibleMaterials.FirstOrDefault(mat => mat.Id == dto.MaterialId);
            if (material == null)
                throw new NotFoundException("possible material");

            PartColorModel color = material.PossibleColors.FirstOrDefault(col => col.Id == dto.ColorId);
            if (color == null)
                throw new NotFoundException("possible color");

            if (ConcretePartRepo.GetPartByMac(dto.ControllerMac) != null)
                throw new ConflictException("mac address");

            ConcretePartModel model = Mapper.Map<AddConcretePartDto, ConcretePartModel>(dto);
            return ProtectedExecute<AddConcretePartDto, ConcretePartModel>(
                concretePart => ConcretePartRepo.Create(concretePart),
                model
            );
        }

        public PartModel RegisterPart(AddPartDto dto)
        {
            AdminService.CheckActiveSuperAdmin(dto.SuperAdminSession);
            PartModel model = Mapper.Map<AddPartDto, PartModel>(dto);
            CheckPinConflict(model);

            return ProtectedExecute<AddPartDto, PartModel>(part => PartRepo.Create(part), model);
        }

        public PartModel UpdatePart(UpdatePartDto dto)
        {
            AdminService.CheckActiveSuperAdmin(dto.SuperAdminSession);
            PartModel model = Mapper.Map<UpdatePartDto, PartModel>(dto);
            CheckPinConflict(model);

            return ProtectedExecute<UpdatePartDto, PartModel>(
                part => PartRepo.Update(part.Id, part), 
                model
            );
        }

        private void CheckPinConflict(PartModel part)
        {
            List<int> usedPins = part.ConnectionHelpers.Aggregate(
                new List<int>(),
                (acc, helper) => acc.Append(helper.IndicatorPinNumber).Append(helper.ReaderPinNumber).ToList()
            );

            if (usedPins.Distinct().Count() != usedPins.Count())
                throw new ConflictException("pin");
        }
    }
}
