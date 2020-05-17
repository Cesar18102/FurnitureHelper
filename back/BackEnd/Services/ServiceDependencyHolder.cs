using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

using Autofac;
using AutoMapper;

using Models;
using Models.Trade;
using Services.Payment;
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
            builder.RegisterType<LiqPayPaymentService>().As<PaymentService>().SingleInstance();

            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            builder.RegisterType<AdminService>().As<IAdminService>().AsSelf().SingleInstance();

            builder.RegisterType<ColorsService>().As<IColorService>().SingleInstance();
            builder.RegisterType<MaterialService>().As<IMaterialService>().SingleInstance();
            builder.RegisterType<PartService>().As<IPartService>().SingleInstance();
            builder.RegisterType<FurnitureService>().As<IFurnitureService>().SingleInstance();
            builder.RegisterType<TradeService>().As<ITradeService>().SingleInstance();

            MapperConfiguration config = new MapperConfiguration(cnf =>
            {
                cnf.AllowNullCollections = true;
                ConfigMapper(cnf);
            });

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

            /*****************/

            config.CreateMap<AddColorDto, PartColorModel>();
            config.CreateMap<UpdateColorDto, PartColorModel>();

            config.CreateMap<int, PartColorModel>()
                  .ForMember(model => model.Id, cnf => cnf.MapFrom(id => id));

            /*****************/

            config.CreateMap<AddMaterialDto, MaterialModel>()
                  .ForMember(model => model.PossibleColors, cnf => cnf.MapFrom(dto => dto.PossibleColors));

            config.CreateMap<UpdateMaterialDto, MaterialModel>()
                  .ForMember(model => model.PossibleColors, cnf => cnf.MapFrom(dto => dto.PossibleColors));

            config.CreateMap<int, MaterialModel>()
                  .ForMember(model => model.Id, cnf => cnf.MapFrom(id => id));

            /*****************/

            config.CreateMap<ConnectionHelperDto, ConnectionHelperModel>()
                  .ForMember(model => model.PosX, cnf => cnf.MapFrom(dto => dto.PosX.GetValueOrDefault()))
                  .ForMember(model => model.PosY, cnf => cnf.MapFrom(dto => dto.PosY.GetValueOrDefault()))
                  .ForMember(model => model.PosZ, cnf => cnf.MapFrom(dto => dto.PosZ.GetValueOrDefault()))
                  .ForMember(model => model.IndicatorPinNumber, cnf => cnf.MapFrom(dto => dto.IndicatorPinNumber))
                  .ForMember(model => model.ReaderPinNumber, cnf => cnf.MapFrom(dto => dto.ReaderPinNumber));

            /*****************/

            config.CreateMap<AddPartDto, PartModel>()
                  .ForMember(model => model.PossibleMaterials, cnf => cnf.MapFrom(dto => dto.PossibleMaterials))
                  .ForMember(model => model.ConnectionHelpers, cnf => cnf.MapFrom(dto => dto.ConnectionHelpers));

            config.CreateMap<UpdatePartDto, PartModel>()
                  .ForMember(model => model.PossibleMaterials, cnf => cnf.MapFrom(dto => dto.PossibleMaterials))
                  .ForMember(model => model.ConnectionHelpers, cnf => cnf.MapFrom(dto => dto.ConnectionHelpers));

            /*****************/

            config.CreateMap<AddFurnitureDto, FurnitureItemModel>()
                  .ForMember(
                      model => model.UsedParts,
                      cnf => cnf.MapFrom(
                          dto => dto.UsedParts.Aggregate(new List<UsedPartModel>(),
                              (acc, parts) => acc.Concat(Enumerable.Repeat(new UsedPartModel(parts.PartId.GetValueOrDefault()), parts.Count.GetValueOrDefault())).ToList()
                          )
                      )
                  );

            config.CreateMap<UpdateFurnitureDto, FurnitureItemModel>()
                  .ForMember(model => model.UsedParts, cnf => cnf.Ignore());

            /********************/

            config.CreateMap<ConnectionGlueDto, ConnectionGlueModel>()
                  .ForPath(model => model.GluePart.Id, cnf => cnf.MapFrom(dto => dto.GluePartId.GetValueOrDefault()))
                  .ForMember(model => model.PosX, cnf => cnf.MapFrom(dto => dto.PosX.GetValueOrDefault()))
                  .ForMember(model => model.PosY, cnf => cnf.MapFrom(dto => dto.PosY.GetValueOrDefault()))
                  .ForMember(model => model.PosZ, cnf => cnf.MapFrom(dto => dto.PosZ.GetValueOrDefault()));

            config.CreateMap<TwoPartsConnectionDto, TwoPartsConnectionModel>()
                  .ForMember(model => model.OrderNumber, cnf => cnf.MapFrom(dto => dto.OrderNumber.GetValueOrDefault()))
                  .ForPath(model => model.ConnectionHelper.Id, cnf => cnf.MapFrom(dto => dto.ConnectionHelperId))
                  .ForPath(model => model.ConnectionHelperOther.Id, cnf => cnf.MapFrom(dto => dto.ConnectionHelperOtherId))
                  .ForPath(model => model.ConnectionGlues, cnf => cnf.MapFrom(dto => dto.ConnectionGlues))
                  .ForMember(model => model.UsedPartId, cnf => cnf.MapFrom(dto => dto.UsedPartId))
                  .ForMember(model => model.UsedPartOtherId, cnf => cnf.MapFrom(dto => dto.UsedPartOtherId));

            config.CreateMap<GlobalConnectionDto, GlobalPartsConnectionModel>()
                  .ForMember(model => model.OrderNumber, cnf => cnf.MapFrom(dto => dto.OrderNumber.GetValueOrDefault()))
                  .ForMember(model => model.GlobalConnectionGlues, cnf => cnf.MapFrom(dto => dto.GlobalConnectionGlues))
                  .ForMember(model => model.SubConnections, cnf => cnf.MapFrom(dto => dto.SubConnections));

            /*****************/

            config.CreateMap<AddConcretePartDto, ConcretePartModel>()
                  .ForPath(model => model.SelectedMaterial.Id, cnf => cnf.MapFrom(dto => dto.MaterialId))
                  .ForPath(model => model.SelectedColor.Id, cnf => cnf.MapFrom(dto => dto.ColorId))
                  .ForPath(model => model.Part.Id, cnf => cnf.MapFrom(dto => dto.PartId))
                  .ForMember(model => model.ControllerMac, cnf => cnf.MapFrom(dto => dto.ControllerMac.ToUpper()));

            /*****************/

            config.CreateMap<AccountExtensionDto, AccountExtensionModel>()
                  .ForMember(model => model.AccountId, cnf => cnf.MapFrom(dto => dto.AccountId.GetValueOrDefault()));

            config.CreateMap<AccountExtensionModel, AccountExtensionModel>();
            config.CreateMap<PaymentConfirmDto, PaymentInfo>();
        }
    }
}