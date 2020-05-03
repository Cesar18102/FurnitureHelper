using System.Text;
using System.Security.Cryptography;

using Autofac;

using ServicesContract;

namespace Services
{
    public static class ServiceDependencyHolder
    {
        public static IContainer ServicesDependencies { get; private set; }

        static ServiceDependencyHolder()
        {
            ContainerBuilder builder = new ContainerBuilder();

            TypedParameter hasher = new TypedParameter(typeof(HashAlgorithm), SHA256.Create());
            TypedParameter encoding = new TypedParameter(typeof(Encoding), Encoding.UTF8);

            builder.RegisterType<HashingService>()
                   .UsingConstructor(typeof(HashAlgorithm), typeof(Encoding))
                   .WithParameters(new TypedParameter[] { hasher, encoding })
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            builder.RegisterType<SessionService>().AsSelf().SingleInstance();

            ServicesDependencies = builder.Build();
        }
    }
}