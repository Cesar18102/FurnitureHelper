namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using MySql.Data.Entity;

    internal sealed class Configuration : DbMigrationsConfiguration<FurnitureHelperContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator(MySqlProviderInvariantName.ProviderName, new MySqlMigrationSqlGenerator());
        }

        protected override void Seed(FurnitureHelperContext context)
        {
        }
    }
}
