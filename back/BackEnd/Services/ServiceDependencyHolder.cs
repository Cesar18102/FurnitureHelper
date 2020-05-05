using System.Text;
using System.Security.Cryptography;

using Autofac;
using AutoMapper;

using Models;
using ServicesContract;
using ServicesContract.Dto;

namespace Services
{
    public class ServiceDependencyHolder
    {
        private static IContainer servicesDependencies = null;
        public static IContainer ServicesDependencies
        {
            get
            {
                if (servicesDependencies == null)
                    servicesDependencies = BuildDependencies();
                return servicesDependencies;
            }
        }

        private ServiceDependencyHolder() { }

        private static IContainer BuildDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            TypedParameter hasher = new TypedParameter(typeof(HashAlgorithm), SHA256.Create());
            TypedParameter encoding = new TypedParameter(typeof(Encoding), Encoding.UTF8);

            builder.RegisterType<HashingService>()
                   .UsingConstructor(typeof(HashAlgorithm), typeof(Encoding))
                   .WithParameters(new TypedParameter[] { hasher, encoding })
                   .AsSelf().SingleInstance();

            builder.RegisterType<SessionService>().AsSelf().SingleInstance();

            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            builder.RegisterType<AdminService>().As<IAdminService>().AsSelf().SingleInstance();

            builder.RegisterType<ColorsService>().As<IColorService>().SingleInstance();
            builder.RegisterType<MaterialService>().As<IMaterialService>().SingleInstance();

            MapperConfiguration config = new MapperConfiguration(cnf => ConfigMapper(cnf));
            TypedParameter mapperConfigParameter = new TypedParameter(typeof(IConfigurationProvider), config);

            builder.RegisterType<Mapper>().AsSelf()
                   .UsingConstructor(typeof(IConfigurationProvider))
                   .WithParameter(mapperConfigParameter).SingleInstance();

            return builder.Build();
        }

        private static void ConfigMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<SignUpDto, AccountModel>();
            config.CreateMap<UpdateAccountDto, AccountModel>();

            config.CreateMap<AddAdminDto, AdminModel>()
                  .ForMember(model => model.Id, cnf => cnf.Ignore())
                  .ForPath(model => model.Account.Id, cnf => cnf.MapFrom(dto => dto.AccountId));

            config.CreateMap<AdminModel, SuperAdminModel>()
                  .ForMember(super => super.Id, cnf => cnf.Ignore())
                  .ForPath(super => super.AdminRights.Id, cnf => cnf.MapFrom(admin => admin.Id));

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

            config.CreateMap<int, PartColorModel>()
                  .ForMember(model => model.Id, cnf => cnf.MapFrom(id => id));

            config.CreateMap<AddMaterialDto, MaterialModel>()
                  .ForMember(model => model.PossibleColors, cnf => cnf.MapFrom(dto => dto.PossibleColors));

            config.CreateMap<UpdateMaterialDto, MaterialModel>()
                  .ForMember(model => model.PossibleColors, cnf => cnf.MapFrom(dto => dto.PossibleColors));
        }
    }
}