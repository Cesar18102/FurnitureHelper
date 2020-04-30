namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using MySql.Data.Entity;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess.FurnitureHelperContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo("FurnitureHelperConnection");
            SetSqlGenerator(MySqlProviderInvariantName.ProviderName, new MySqlMigrationSqlGenerator());
        }


        protected override void Seed(FurnitureHelperContext context)
        {
        }
    }
}
