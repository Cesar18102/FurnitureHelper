using Autofac;

using DataAccess;
using DataAccessContract;
using DataAccess.RepoImplementation;

using DataTypes.Dto;
using DataTypes.Models;

namespace DataAccessHolder
{
    public static class DataAccessDependencyHolder
    {
        public static IContainer DataAccessDependencies { get; private set; }

        static DataAccessDependencyHolder()
        {
            ContainerBuilder builder = new ContainerBuilder();
            FurnitureHelperContext dbContext = new FurnitureHelperContext();

            builder.RegisterType<AccountRepo>()
                   .As<IAccountRepo>()
                   .As<IRepo<AccountDto, AccountModel>>()
                   .UsingConstructor(typeof(FurnitureHelperContext))
                   .WithParameter(new TypedParameter(typeof(FurnitureHelperContext), dbContext))
                   .SingleInstance();

            DataAccessDependencies = builder.Build();
        }
    }
}