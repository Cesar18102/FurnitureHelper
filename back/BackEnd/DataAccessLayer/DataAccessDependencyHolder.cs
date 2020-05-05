﻿using Autofac;
using AutoMapper;

using DataAccessContract;

using DataAccess.Entities;
using DataAccess.RepoImplementation;

using Models;

namespace DataAccess
{
    public class DataAccessDependencyHolder
    {
        private static IContainer dataAccessDependencies = null;
        public static IContainer DataAccessDependencies
        {
            get
            {
                if (dataAccessDependencies == null)
                    dataAccessDependencies = BuildDependencies();
                return dataAccessDependencies;
            }
        }

        private DataAccessDependencyHolder() { }

        private static IContainer BuildDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();
            FurnitureHelperContext dbContext = new FurnitureHelperContext();

            TypedParameter contextParameter = new TypedParameter(typeof(FurnitureHelperContext), dbContext);

            builder.RegisterType<AccountRepo>()
                   .As<IAccountRepo>().As<IRepo<AccountModel>>().AsSelf()
                   .UsingConstructor(typeof(FurnitureHelperContext))
                   .WithParameter(contextParameter).SingleInstance();

            builder.RegisterType<AdminRepo>()
                   .As<IAdminRepo>().As<IRepo<AdminModel>>()
                   .UsingConstructor(typeof(FurnitureHelperContext))
                   .WithParameter(contextParameter).SingleInstance();

            builder.RegisterType<SuperAdminRepo>()
                   .As<ISuperAdminRepo>().As<IRepo<SuperAdminModel>>()
                   .UsingConstructor(typeof(FurnitureHelperContext))
                   .WithParameter(contextParameter).SingleInstance();

            builder.RegisterType<ColorRepo>()
                   .As<IColorRepo>().As<IRepo<PartColorModel>>()
                   .UsingConstructor(typeof(FurnitureHelperContext))
                   .WithParameter(contextParameter).SingleInstance();

            builder.RegisterType<MaterialRepo>()
                   .As<IMaterialRepo>().As<IRepo<MaterialModel>>()
                   .UsingConstructor(typeof(FurnitureHelperContext))
                   .WithParameter(contextParameter).SingleInstance();

            builder.RegisterType<PartRepo>()
                   .As<IPartRepo>().As<IRepo<PartModel>>()
                   .UsingConstructor(typeof(FurnitureHelperContext))
                   .WithParameter(contextParameter).SingleInstance();

            MapperConfiguration config = new MapperConfiguration(cnf => ConfigMapper(cnf, dbContext));
            TypedParameter mapperConfigParameter = new TypedParameter(typeof(IConfigurationProvider), config);

            builder.RegisterType<Mapper>().AsSelf()
                   .UsingConstructor(typeof(IConfigurationProvider))
                   .WithParameter(mapperConfigParameter).SingleInstance();
            
            return builder.Build();
        }

        private static void ConfigMapper(IMapperConfigurationExpression config, FurnitureHelperContext ctx)
        {
            config.CreateMap<AccountModel, AccountEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.accounts_extensions, cnf => cnf.MapFrom(model => model.AccountExtensions))
                  .ForMember(entity => entity.first_name, cnf => cnf.MapFrom(model => model.FirstName))
                  .ForMember(entity => entity.last_name, cnf => cnf.MapFrom(model => model.LastName))
                  .ForMember(entity => entity.pwd, cnf => cnf.MapFrom(model => model.Password))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<AccountEntity, AccountModel>()
                  .ForMember(model => model.Password, cnf => cnf.MapFrom(entity => entity.pwd))
                  .ForMember(model => model.FirstName, cnf => cnf.MapFrom(entity => entity.first_name))
                  .ForMember(model => model.LastName, cnf => cnf.MapFrom(entity => entity.last_name))
                  .ForMember(model => model.AccountExtensions, cnf => cnf.MapFrom(entity => entity.accounts_extensions));

            config.CreateMap<AdminModel, AdminEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.account_id, cnf => cnf.MapFrom(model => model.Account.Id))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<AdminEntity, AdminModel>()
                  .ForMember(model => model.Id, cnf => cnf.MapFrom(entity => entity.id))
                  .ForMember(model => model.Account, cnf => cnf.MapFrom(entity => entity.accounts));

            config.CreateMap<SuperAdminModel, SuperAdminEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.admin_account_id, cnf => cnf.MapFrom(model => model.AdminRights.Id))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<SuperAdminEntity, SuperAdminModel>()
                  .ForMember(model => model.Id, cnf => cnf.MapFrom(entity => entity.id))
                  .ForMember(model => model.AdminRights, cnf => cnf.MapFrom(entity => entity.admins));

            config.CreateMap<PartColorModel, PartColorEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<PartColorEntity, PartColorModel>();

            config.CreateMap<MaterialModel, MaterialEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.texture_url, cnf => cnf.MapFrom(model => model.TextureUrl))
                  .ForMember(entity => entity.price_coeff, cnf => cnf.MapFrom(model => model.PriceCoefficient))
                  .ForMember(entity => entity.colors, cnf => cnf.Ignore())
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<MaterialEntity, MaterialModel>()
                  .ForMember(model => model.TextureUrl, cnf => cnf.MapFrom(entity => entity.texture_url))
                  .ForMember(model => model.PriceCoefficient, cnf => cnf.MapFrom(entity => entity.price_coeff))
                  .ForMember(model => model.PossibleColors, cnf => cnf.MapFrom(entity => entity.colors));

            config.CreateMap<EmbedControllerPositionModel, PartControllerEmbedRelativePositionEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.pos_x, cnf => cnf.MapFrom(model => model.PosX))
                  .ForMember(entity => entity.pos_y, cnf => cnf.MapFrom(model => model.PosY))
                  .ForMember(entity => entity.pos_z, cnf => cnf.MapFrom(model => model.PosZ))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<PartControllerEmbedRelativePositionEntity, EmbedControllerPositionModel>()
                  .ForMember(model => model.PosX, cnf => cnf.MapFrom(entity => entity.pos_x))
                  .ForMember(model => model.PosY, cnf => cnf.MapFrom(entity => entity.pos_y))
                  .ForMember(model => model.PosZ, cnf => cnf.MapFrom(entity => entity.pos_z));

            config.CreateMap<PartModel, PartEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.model_url, cnf => cnf.MapFrom(model => model.ModelUrl))
                  .ForMember(entity => entity.materials, cnf => cnf.Ignore())
                  .ForMember(
                      entity => entity.part_controllers_embed_relative_positions, 
                      cnf => cnf.MapFrom(model => model.EmbedControllersPositions)
                  ).ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<PartEntity, PartModel>()
                  .ForMember(model => model.ModelUrl, cnf => cnf.MapFrom(entity => entity.model_url))
                  .ForMember(model => model.PossibleMaterials, cnf => cnf.MapFrom(entity => entity.materials))
                  .ForMember(model => model.EmbedControllersPositions, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions));
        }
    }
}