using Autofac;
using AutoMapper;

using DataAccessContract;

using DataAccess.Entities;
using DataAccess.RepoImplementation;

using Models;
using System;

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

            builder.RegisterType<AccountExtensionRepo>()
                   .As<IAccountExtensionRepo>().As<IRepo<AccountExtensionModel>>().AsSelf()
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

            builder.RegisterType<ManufacturerSellRepo>()
                  .As<IManufacturerSellsRepo>().As<IRepo<SellModel>>()
                  .UsingConstructor(typeof(FurnitureHelperContext))
                  .WithParameter(contextParameter).SingleInstance();

            builder.RegisterType<OwnershipRepo>()
                  .As<IOwnershipRepo>().As<IRepo<OwnershipModel>>()
                  .UsingConstructor(typeof(FurnitureHelperContext))
                  .WithParameter(contextParameter).SingleInstance();

            MapperConfiguration config = new MapperConfiguration(cnf =>
            {
                cnf.AllowNullCollections = true;
                ConfigMapper(cnf, dbContext);
            });

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
                  .ForMember(entity => entity.accounts_extensions, cnf => cnf.MapFrom(model => model.AccountExtensions))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<AccountEntity, AccountModel>()
                  .ForMember(model => model.Password, cnf => cnf.MapFrom(entity => entity.pwd))
                  .ForMember(model => model.FirstName, cnf => cnf.MapFrom(entity => entity.first_name))
                  .ForMember(model => model.LastName, cnf => cnf.MapFrom(entity => entity.last_name))
                  .ForMember(model => model.AccountExtensions, cnf => cnf.MapFrom(entity => entity.accounts_extensions));

            config.CreateMap<AccountExtensionModel, AccountExtensionEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.account_id, cnf => cnf.MapFrom(model => model.AccountId))
                  .ForMember(entity => entity.last_used, cnf => cnf.MapFrom(model => model.LastUsedDate));

            config.CreateMap<AccountExtensionEntity, AccountExtensionModel>()
                  .ForMember(model => model.AccountId, cnf => cnf.MapFrom(entity => entity.account_id))
                  .ForMember(model => model.LastUsedDate, cnf => cnf.MapFrom(entity => entity.last_used));

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
                  .ForMember(entity => entity.price_coeff, cnf => cnf.MapFrom((model, entity) => 
                        model.PriceCoefficient == null || !model.PriceCoefficient.HasValue ? entity.price_coeff : model.PriceCoefficient.Value)
                  ).ForMember(entity => entity.colors, cnf => cnf.Ignore())
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<MaterialEntity, MaterialModel>()
                  .ForMember(model => model.TextureUrl, cnf => cnf.MapFrom(entity => entity.texture_url))
                  .ForMember(model => model.PriceCoefficient, cnf => cnf.MapFrom(entity => entity.price_coeff))
                  .ForMember(model => model.PossibleColors, cnf => cnf.MapFrom(entity => entity.colors));

            /******************/

            config.CreateMap<ConnectionHelperModel, PartControllerEmbedRelativePositionEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.pos_x, cnf => cnf.MapFrom(model => model.PosX))
                  .ForMember(entity => entity.pos_y, cnf => cnf.MapFrom(model => model.PosY))
                  .ForMember(entity => entity.pos_z, cnf => cnf.MapFrom(model => model.PosZ))
                  .ForMember(entity => entity.pos_x_other, cnf => cnf.MapFrom(model => model.PosXOther))
                  .ForMember(entity => entity.pos_y_other, cnf => cnf.MapFrom(model => model.PosYOther))
                  .ForMember(entity => entity.pos_z_other, cnf => cnf.MapFrom(model => model.PosZOther))
                  .ForMember(entity => entity.pos_x_help, cnf => cnf.MapFrom(model => model.PosXHelp))
                  .ForMember(entity => entity.pos_y_help, cnf => cnf.MapFrom(model => model.PosYHelp))
                  .ForMember(entity => entity.pos_z_help, cnf => cnf.MapFrom(model => model.PosZHelp))
                  .ForMember(entity => entity.indicator_pin_number, cnf => cnf.MapFrom(model => model.IndicatorPinNumber))
                  .ForMember(entity => entity.reader_pin_number, cnf => cnf.MapFrom(model => model.ReaderPinNumber))
                  .ForMember(entity => entity.reader_pin_number_other, cnf => cnf.MapFrom(model => model.ReaderPinNumberOther))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<PartControllerEmbedRelativePositionEntity, ConnectionHelperModel>()
                  .ForMember(model => model.PosX, cnf => cnf.MapFrom(entity => entity.pos_x))
                  .ForMember(model => model.PosY, cnf => cnf.MapFrom(entity => entity.pos_y))
                  .ForMember(model => model.PosZ, cnf => cnf.MapFrom(entity => entity.pos_z))
                  .ForMember(model => model.PosXOther, cnf => cnf.MapFrom(entity => entity.pos_x_other))
                  .ForMember(model => model.PosYOther, cnf => cnf.MapFrom(entity => entity.pos_y_other))
                  .ForMember(model => model.PosZOther, cnf => cnf.MapFrom(entity => entity.pos_z_other))
                  .ForMember(model => model.PosXHelp, cnf => cnf.MapFrom(entity => entity.pos_x_help))
                  .ForMember(model => model.PosYHelp, cnf => cnf.MapFrom(entity => entity.pos_y_help))
                  .ForMember(model => model.PosZHelp, cnf => cnf.MapFrom(entity => entity.pos_z_help))
                  .ForMember(model => model.IndicatorPinNumber, cnf => cnf.MapFrom(entity => entity.indicator_pin_number))
                  .ForMember(model => model.ReaderPinNumber, cnf => cnf.MapFrom(entity => entity.reader_pin_number))
                  .ForMember(model => model.ReaderPinNumberOther, cnf => cnf.MapFrom(entity => entity.reader_pin_number_other));

            config.CreateMap<PartModel, PartEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.model_url, cnf => cnf.MapFrom(model => model.ModelUrl))
                  .ForMember(entity => entity.materials, cnf => cnf.Ignore())
                  .ForMember(entity => entity.price, cnf => cnf.MapFrom((model, entity) => model.Price == null || !model.Price.HasValue ? entity.price : model.Price.Value))
                  .ForMember(entity => entity.scale, cnf => cnf.MapFrom((model, entity) => model.Scale == null || !model.Scale.HasValue ? entity.scale : model.Scale.Value))
                  .ForMember(entity => entity.in_furniture_scale, cnf => cnf.MapFrom((model, entity) => model.InFurnitureScale == null || !model.InFurnitureScale.HasValue ? entity.in_furniture_scale : model.InFurnitureScale.Value))
                  .ForMember(entity => entity.part_controllers_embed_relative_positions, cnf => cnf.MapFrom(model => model.ConnectionHelpers))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<PartEntity, PartModel>()
                  .ForMember(model => model.ModelUrl, cnf => cnf.MapFrom(entity => entity.model_url))
                  .ForMember(model => model.PossibleMaterials, cnf => cnf.MapFrom(entity => entity.materials))
                  .ForMember(model => model.InFurnitureScale, cnf => cnf.MapFrom(entity => entity.in_furniture_scale))
                  .ForMember(model => model.ConnectionHelpers, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions));

            /*******************/

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
                  .ForMember(entity => entity.part_controller_id, cnf => cnf.MapFrom(model => model.ConnectionHelper.Id))
                  .ForMember(entity => entity.part_controller_other_id, cnf => cnf.MapFrom(model => model.ConnectionHelperOther.Id))
                  .ForMember(entity => entity.two_parts_connection_glues, cnf => cnf.MapFrom(model => model.ConnectionGlues))
                  .ForMember(entity => entity.used_part_id, cnf => cnf.MapFrom(model => model.UsedPartId))
                  .ForMember(entity => entity.used_part_other_id, cnf => cnf.MapFrom(model => model.UsedPartOtherId))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<GlobalPartsConnectionModel, FurnitureItemPartsConnectionEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.comment_text, cnf => cnf.MapFrom(model => model.Comment))
                  .ForMember(entity => entity.order_number, cnf => cnf.MapFrom(model => model.OrderNumber))
                  .ForMember(entity => entity.parts_connection_glues, cnf => cnf.MapFrom(model => model.GlobalConnectionGlues))
                  .ForMember(entity => entity.two_parts_connection, cnf => cnf.MapFrom(model => model.SubConnections))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            /***/

            config.CreateMap<TwoPartsConnectionGlueEntity, ConnectionGlueModel>()
                 .ForMember(model => model.Comment, cnf => cnf.MapFrom(entity => entity.comment_text))
                 .ForPath(model => model.GluePart, cnf => cnf.MapFrom(entity => entity.parts))
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
                  .ForMember(model => model.ConnectionHelper, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions))
                  .ForMember(model => model.ConnectionHelperOther, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions1))
                  .ForMember(model => model.ConnectionGlues, cnf => cnf.MapFrom(entity => entity.two_parts_connection_glues))
                  .ForMember(model => model.Part, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions.parts))
                  .ForMember(model => model.PartOther, cnf => cnf.MapFrom(entity => entity.part_controllers_embed_relative_positions1.parts))
                  .ForMember(model => model.UsedPartId, cnf => cnf.MapFrom(entity => entity.used_part_id))
                  .ForMember(model => model.UsedPartOtherId, cnf => cnf.MapFrom(entity => entity.used_part_other_id))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<FurnitureItemPartsConnectionEntity, GlobalPartsConnectionModel>()
                  .ForMember(model => model.Comment, cnf => cnf.MapFrom(entity => entity.comment_text))
                  .ForMember(model => model.OrderNumber, cnf => cnf.MapFrom(entity => entity.order_number))
                  .ForMember(model => model.SubConnections, cnf => cnf.MapFrom(entity => entity.two_parts_connection))
                  .ForMember(model => model.GlobalConnectionGlues, cnf => cnf.MapFrom(entity => entity.parts_connection_glues));

            /******************/

            config.CreateMap<UsedPartModel, UsedPartEntity>()
                 .ForMember(entity => entity.id, cnf => cnf.Ignore())
                 .ForMember(entity => entity.furniture_item_id, cnf => cnf.MapFrom(model => model.FurnitureItemId))
                 .ForMember(entity => entity.part_id, cnf => cnf.MapFrom(model => model.PartId))
                 .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<FurnitureItemModel, FurnitureItemEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.scale, cnf => cnf.MapFrom((model, entity) => model.Scale == null || !model.Scale.HasValue ? entity.scale : model.Scale.Value))
                  .ForMember(entity => entity.used_parts, cnf => cnf.MapFrom(model => model.UsedParts))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<UsedPartEntity, UsedPartModel>()
                  .ForMember(model => model.PartId, cnf => cnf.MapFrom(entity => entity.part_id))
                  .ForMember(model => model.FurnitureItemId, cnf => cnf.MapFrom(entity => entity.furniture_item_id));

            config.CreateMap<FurnitureItemEntity, FurnitureItemModel>()
                  .ForMember(model => model.GlobalConnections, cnf => cnf.MapFrom(entity => entity.furniture_item_parts_connections))
                  .ForMember(model => model.UsedParts, cnf => cnf.MapFrom(entity => entity.used_parts));

            /******************/

            config.CreateMap<ConcretePartModel, ConcretePartEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.part_id, cnf => cnf.MapFrom(model => model.Part.Id))
                  .ForMember(entity => entity.controller_mac, cnf => cnf.MapFrom(model => model.ControllerMac.ToUpper()))
                  .ForMember(entity => entity.material_id, cnf => cnf.MapFrom(model => model.SelectedMaterial.Id))
                  .ForMember(entity => entity.color_id, cnf => cnf.MapFrom(model => model.SelectedColor.Id))
                  .ForMember(entity => entity.create_date, cnf => cnf.MapFrom(model => model.CreateDate))
                  .ForMember(entity => entity.last_sell_date, cnf => cnf.MapFrom(model => model.LastSellDate))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<ConcretePartEntity, ConcretePartModel>()
                  .ForMember(model => model.Part, cnf => cnf.MapFrom(entity => entity.parts))
                  .ForMember(model => model.ControllerMac, cnf => cnf.MapFrom(entity => entity.controller_mac.ToUpper()))
                  .ForMember(model => model.SelectedMaterial, cnf => cnf.MapFrom(entity => entity.materials))
                  .ForMember(model => model.SelectedColor, cnf => cnf.MapFrom(entity => entity.colors))
                  .ForMember(model => model.CreateDate, cnf => cnf.MapFrom(entity => entity.create_date))
                  .ForMember(model => model.LastSellDate, cnf => cnf.MapFrom(entity => entity.last_sell_date))
                  .ForMember(model => model.IsInUse, cnf => cnf.MapFrom(entity => entity.in_use))
                  .ForMember(model => model.IsForSell, cnf => cnf.MapFrom(entity => entity.for_sell));

            /******************/

            config.CreateMap<SellPositionModel, ManufacturerSellPositionEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.concrete_part_id, cnf => cnf.MapFrom(model => model.ConcretePart.Id))
                  .ForMember(entity => entity.price, cnf => cnf.MapFrom(model => model.Price));

            config.CreateMap<ManufacturerSellPositionEntity, SellPositionModel>()
                  .ForMember(model => model.ConcretePart, cnf => cnf.MapFrom(entity => entity.concrete_parts));

            config.CreateMap<SellModel, ManufacturerSellEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.accounts_extension_id, cnf => cnf.MapFrom(model => model.BuyerAccountExtension.Id))
                  .ForMember(entity => entity.manufacturer_sell_positions, cnf => cnf.MapFrom(model => model.SellPositions))
                  .ForMember(entity => entity.sell_date, cnf => cnf.MapFrom(model => model.SellDate))
                  .ForAllMembers(cnf => cnf.Condition((entity, model, member) => member != null));

            config.CreateMap<ManufacturerSellEntity, SellModel>()
                  .ForMember(model => model.BuyerAccountExtension, cnf => cnf.MapFrom(entity => entity.accounts_extensions))
                  .ForMember(model => model.SellPositions, cnf => cnf.MapFrom(entity => entity.manufacturer_sell_positions))
                  .ForMember(model => model.SellDate, cnf => cnf.MapFrom(entity => entity.sell_date));

            /******************/

            config.CreateMap<OwnershipModel, OwningEntity>()
                  .ForMember(entity => entity.id, cnf => cnf.Ignore())
                  .ForMember(entity => entity.account_id, cnf => cnf.MapFrom(model => model.AccountId))
                  .ForMember(entity => entity.concrete_part_id, cnf => cnf.MapFrom(model => model.ConcretePart.Id));

            config.CreateMap<OwningEntity, OwnershipModel>()
                  .ForMember(model => model.AccountId, cnf => cnf.MapFrom(entity => entity.account_id))
                  .ForMember(model => model.ConcretePart, cnf => cnf.MapFrom(entity => entity.concrete_parts));
        }
    }
}