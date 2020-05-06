using Autofac;
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

            builder.RegisterType<FurnitureRepo>()
                  .As<IFurnitureRepo>().As<IRepo<FurnitureItemModel>>()
                  .UsingConstructor(typeof(FurnitureHelperContext))
                  .WithParameter(contextParameter).SingleInstance();

            builder.RegisterType<ConcretePartRepo>()
                  .As<IConcretePartRepo>().As<IRepo<ConcretePartModel>>()
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

            /******************/

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

            /******************/

            config.CreateMap<PartColorModel, PartColorEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<PartColorEntity, PartColorModel>();

            /******************/

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

            /******************/

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

            /******************/

            config.CreateMap<ConnectionGlueModel, TwoPartsConnectionGlueEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.comment_text, cnf => cnf.MapFrom(model => model.Comment))
                  .ForMember(entity => entity.glue_part_id, cnf => cnf.MapFrom(model => model.GluePart.Id))
                  .ForMember(entity => entity.pos_x, cnf => cnf.MapFrom(model => model.PosX))
                  .ForMember(entity => entity.pos_y, cnf => cnf.MapFrom(model => model.PosY))
                  .ForMember(entity => entity.pos_z, cnf => cnf.MapFrom(model => model.PosZ))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<ConnectionGlueModel, PartsConnectionGlueEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.comment_text, cnf => cnf.MapFrom(model => model.Comment))
                  .ForMember(entity => entity.glue_part_id, cnf => cnf.MapFrom(model => model.GluePart.Id))
                  .ForMember(entity => entity.pos_x, cnf => cnf.MapFrom(model => model.PosX))
                  .ForMember(entity => entity.pos_y, cnf => cnf.MapFrom(model => model.PosY))
                  .ForMember(entity => entity.pos_z, cnf => cnf.MapFrom(model => model.PosZ))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<TwoPartsConnectionModel, TwoPartsConnectionEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.comment_text, cnf => cnf.MapFrom(model => model.Comment))
                  .ForMember(entity => entity.order_number, cnf => cnf.MapFrom(model => model.OrderNumber))
                  .ForMember(entity => entity.part_controller_id, cnf => cnf.MapFrom(model => model.ControllerPosition.Id))
                  .ForMember(entity => entity.part_controller_other_id, cnf => cnf.MapFrom(model => model.ControllerPositionOther.Id))
                  .ForMember(entity => entity.two_parts_connection_glues, cnf => cnf.MapFrom(model => model.ConnectionGlues))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<GlobalPartsConnectionModel, FurnitureItemPartsConnectionEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.comment_text, cnf => cnf.MapFrom(model => model.Comment))
                  .ForMember(entity => entity.order_number, cnf => cnf.MapFrom(model => model.OrderNumber))
                  .ForMember(entity => entity.parts_connection_glues, cnf => cnf.MapFrom(model => model.GlobalConnectionGlues))
                  .ForMember(entity => entity.two_parts_connection, cnf => cnf.MapFrom(model => model.SubConnections))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<FurnitureItemModel, FurnitureItemEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.furniture_item_parts_connections, cnf => cnf.MapFrom(model => model.GlobalConnections))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));


            /******************/

            config.CreateMap<TwoPartsConnectionGlueEntity, ConnectionGlueModel>()
                 .ForMember(model => model.Comment, cnf => cnf.MapFrom(entity => entity.comment_text))
                 .ForPath(model => model.GluePart.Id, cnf => cnf.MapFrom(entity => entity.glue_part_id.GetValueOrDefault()))
                 .ForMember(model => model.PosX, cnf => cnf.MapFrom(entity => entity.pos_x))
                 .ForMember(model => model.PosY, cnf => cnf.MapFrom(entity => entity.pos_y))
                 .ForMember(model => model.PosZ, cnf => cnf.MapFrom(entity => entity.pos_z));

            config.CreateMap<PartsConnectionGlueEntity, ConnectionGlueModel>()
                  .ForMember(model => model.Comment, cnf => cnf.MapFrom(entity => entity.comment_text))
                  .ForMember(model => model.GluePart, cnf => cnf.MapFrom(entity => entity.parts))
                  .ForMember(model => model.PosX, cnf => cnf.MapFrom(entity => entity.pos_x))
                  .ForMember(model => model.PosY, cnf => cnf.MapFrom(entity => entity.pos_y))
                  .ForMember(model => model.PosZ, cnf => cnf.MapFrom(entity => entity.pos_z));

            config.CreateMap<TwoPartsConnectionEntity, TwoPartsConnectionModel>()
                  .ForMember(model => model.Comment, cnf => cnf.MapFrom(entity => entity.comment_text))
                  .ForMember(model => model.OrderNumber, cnf => cnf.MapFrom(entity => entity.order_number))
                  .ForMember(model => model.ControllerPosition, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions))
                  .ForMember(model => model.ControllerPositionOther, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions1))
                  .ForMember(model => model.ConnectionGlues, cnf => cnf.MapFrom(entity => entity.two_parts_connection_glues))
                  .ForMember(model => model.Part, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions.parts))
                  .ForMember(model => model.PartOther, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions1.parts));

            config.CreateMap<FurnitureItemPartsConnectionEntity, GlobalPartsConnectionModel>()
                  .ForMember(model => model.Comment, cnf => cnf.MapFrom(entity => entity.comment_text))
                  .ForMember(model => model.OrderNumber, cnf => cnf.MapFrom(entity => entity.order_number))
                  .ForMember(model => model.SubConnections, cnf => cnf.MapFrom(entity => entity.two_parts_connection))
                  .ForMember(model => model.GlobalConnectionGlues, cnf => cnf.MapFrom(entity => entity.parts_connection_glues));

            config.CreateMap<FurnitureItemEntity, FurnitureItemModel>()
                  .ForMember(model => model.GlobalConnections, cnf => cnf.MapFrom(entity => entity.furniture_item_parts_connections));

            /******************/

            config.CreateMap<ConcreteControllerModel, ConcreteControllerEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.mac, cnf => cnf.MapFrom(model => model.MAC.ToUpper()))
                  .ForMember(entity => entity.embed_position_id, cnf => cnf.MapFrom(model => model.EmbedPosition.Id))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<ConcretePartModel, ConcretePartEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.part_id, cnf => cnf.MapFrom(model => model.Part.Id))
                  .ForMember(entity => entity.material_id, cnf => cnf.MapFrom(model => model.SelectedMaterial.Id))
                  .ForMember(entity => entity.color_id, cnf => cnf.MapFrom(model => model.SelectedColor.Id))
                  .ForMember(entity => entity.create_date, cnf => cnf.MapFrom(model => model.CreateDate))
                  .ForMember(entity => entity.concrete_controllers, cnf => cnf.MapFrom(model => model.EmbedControllers))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            /******************/

            config.CreateMap<ConcreteControllerEntity, ConcreteControllerModel>()
                 .ForMember(model => model.MAC, cnf => cnf.MapFrom(entity => entity.mac.ToUpper()))
                 .ForMember(model => model.EmbedPosition, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions));

            config.CreateMap<ConcretePartEntity, ConcretePartModel>()
                  .ForMember(model => model.Part, cnf => cnf.MapFrom(entity => entity.parts))
                  .ForMember(model => model.SelectedMaterial, cnf => cnf.MapFrom(entity => entity.materials))
                  .ForMember(model => model.SelectedColor, cnf => cnf.MapFrom(entity => entity.colors))
                  .ForMember(model => model.CreateDate, cnf => cnf.MapFrom(entity => entity.create_date))
                  .ForMember(model => model.EmbedControllers, cnf => cnf.MapFrom(entity => entity.concrete_controllers));

            /******************/
        }
    }
}