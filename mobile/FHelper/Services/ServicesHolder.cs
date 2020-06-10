using Autofac;

using Services.Declaration;
using Services.Implementation;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public class ServicesHolder
    {
        private static IContainer Container = null;
        public static IContainer Dependencies
        {
            get
            {
                if (Container == null)
                    Container = InitDependencies();
                return Container;
            }
        }

        private ServicesHolder() { }

        private static IContainer InitDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            TypedParameter hasher = new TypedParameter(typeof(HashAlgorithm), SHA256.Create());
            TypedParameter encoding = new TypedParameter(typeof(Encoding), Encoding.UTF8);

            builder.RegisterType<HashingService>()
                   .UsingConstructor(typeof(HashAlgorithm), typeof(Encoding))
                   .WithParameters(new TypedParameter[] { hasher, encoding })
                   .AsSelf().SingleInstance();

            builder.RegisterType<AuthorizeService>().As<IAuthorizeService>().SingleInstance();
            builder.RegisterType<PartService>().As<IPartService>().SingleInstance();
            builder.RegisterType<FurnitureService>().As<IFurnitureService>().SingleInstance();

            return builder.Build();
        }
    }
}