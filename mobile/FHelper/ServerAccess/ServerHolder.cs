using Autofac;

namespace ServerAccess
{
    public class ServerHolder
    {
        public const string SERVER_URL = "http://furniturehelper.somee.com/api/";

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

        private ServerHolder() { }

        private static IContainer InitDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Server>().As<IServer>().SingleInstance();

            return builder.Build();
        }
    }
}
