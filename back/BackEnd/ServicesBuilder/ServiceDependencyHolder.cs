using Autofac;

using Services;
using ServicesContract;

namespace ServiceHolder
{
    public static class ServiceDependencyHolder
    {
        public static IContainer ServicesDependencies { get; private set; }

        static ServiceDependencyHolder()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();

            ServicesDependencies = builder.Build();
        }
    }
}