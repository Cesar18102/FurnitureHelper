using Autofac;
using AutoMapper;

using Models;
using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

using DataAccessHolder;
using DataAccessContract;

namespace Services
{
    public class ColorsService : ServiceBase, IColorService
    {
        private static readonly IColorRepo ColorRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IColorRepo>();
        private static readonly AdminService AdminService = ServiceDependencyHolder.ServicesDependencies.Resolve<AdminService>();

        public PartColorModel RegisterColor(AddColorDto dto)
        {
            AdminService.CheckActiveSuperAdmin(dto.SuperAdminSession);

            PartColorModel model = Mapper.Map<AddColorDto, PartColorModel>(dto);

            if (ColorRepo.GetByName(model.Name) != null)
                throw new ConflictException("Color name");

            return ProtectedExecute<AddColorDto, PartColorModel>(colorModel => ColorRepo.Create(colorModel), model);
        }

        public PartColorModel UpdateColor(UpdateColorDto dto)
        {
            AdminService.CheckActiveSuperAdmin(dto.SuperAdminSession);

            PartColorModel model = Mapper.Map<UpdateColorDto, PartColorModel>(dto);
            PartColorModel foundColor = ColorRepo.GetByName(model.Name);

            if (foundColor != null && foundColor.Id != model.Id)
                throw new ConflictException("Color name");

            return ProtectedExecute<UpdateColorDto, PartColorModel>(
                colorModel => ColorRepo.Update(colorModel.Id, colorModel), 
                model
            );
        }
    }
}