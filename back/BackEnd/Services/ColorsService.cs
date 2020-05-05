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

        private static readonly SessionService SessionService = ServiceDependencyHolder.ServicesDependencies.Resolve<SessionService>();
        private static readonly AdminService AdminService = ServiceDependencyHolder.ServicesDependencies.Resolve<AdminService>();

        protected override void ConfigDtoModelMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<AddColorDto, PartColorModel>()
                  .ForMember(
                        model => model.Hex,
                        cnf => cnf.MapFrom(dto => (dto.Red * 256 * 256 + dto.Green * 256 + dto.Blue).ToString("X6"))
                  );

            config.CreateMap<UpdateColorDto, PartColorModel>()
                  .ForMember(
                        model => model.Hex,
                        cnf => cnf.MapFrom(dto => (dto.Red * 256 * 256 + dto.Green * 256 + dto.Blue).ToString("X6"))
                  );
        }

        public PartColorModel RegisterColor(AddColorDto dto)
        {
            SessionService.CheckSession(dto.AdminSession);

            if (!AdminService.IsSuperAdmin(dto.AdminSession.UserId))
                throw new ForbiddenException("Superadmin");

            PartColorModel model = Mapper.Map<AddColorDto, PartColorModel>(dto);

            if (ColorRepo.GetByName(model.Name) != null)
                throw new ConflictException("Color name");

            return ProtectedExecute<AddColorDto, PartColorModel>(colorModel => ColorRepo.Create(colorModel), model);
        }

        public PartColorModel UpdateColor(UpdateColorDto dto)
        {
            SessionService.CheckSession(dto.AdminSession);

            if (!AdminService.IsSuperAdmin(dto.AdminSession.UserId))
                throw new ForbiddenException("Superadmin");

            PartColorModel model = Mapper.Map<UpdateColorDto, PartColorModel>(dto);
            PartColorModel foundColor = ColorRepo.GetByName(model.Name);

            if (foundColor != null && foundColor.Id != model.Id)
                throw new ConflictException("Color name");

            PartColorModel updatedColor = ProtectedExecute<UpdateColorDto, PartColorModel>(
                colorModel => ColorRepo.Update(colorModel.Id, colorModel), 
                model
            );

            if (updatedColor == null)
                throw new NotFoundException("Color");

            return updatedColor;
        }
    }
}