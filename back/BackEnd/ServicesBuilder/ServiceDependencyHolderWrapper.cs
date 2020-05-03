using Autofac;

using Services;

namespace ServiceHolder
{
    public static class ServiceDependencyHolderWrapper
    {
        public static IContainer ServicesDependencies => ServiceDependencyHolder.ServicesDependencies;
    }
}