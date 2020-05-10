using System.Collections.Generic;

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
            return ProtectedExecute<AddColorDto, PartColorModel>(colorDto =>
            {
                AdminService.CheckActiveSuperAdmin(colorDto.SuperAdminSession);
                PartColorModel model = Mapper.Map<AddColorDto, PartColorModel>(colorDto);

                if (ColorRepo.GetByName(model.Name) != null)
                    throw new ConflictException("Color name");

                return ColorRepo.Create(model);
            }, dto);
        }

        public PartColorModel UpdateColor(UpdateColorDto dto)
        {
            return ProtectedExecute<UpdateColorDto, PartColorModel>(colorDto =>
            {
                AdminService.CheckActiveSuperAdmin(colorDto.SuperAdminSession);

                PartColorModel model = Mapper.Map<UpdateColorDto, PartColorModel>(colorDto);
                PartColorModel foundColor = ColorRepo.GetByName(model.Name);

                if (foundColor != null && foundColor.Id != model.Id)
                    throw new ConflictException("Color name");

                return ColorRepo.Update(model.Id, model);
            }, dto);
        }

        public IEnumerable<PartColorModel> GetAll()
        {
            return ColorRepo.GetAll();
        }
    }
}