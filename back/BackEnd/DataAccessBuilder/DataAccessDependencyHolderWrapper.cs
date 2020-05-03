using Autofac;

using DataAccess;

namespace DataAccessHolder
{
    public static class DataAccessDependencyHolderWrapper
    {
        public static IContainer DataAccessDependencies => DataAccessDependencyHolder.DataAccessDependencies;
    }
}