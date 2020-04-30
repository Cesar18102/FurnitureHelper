namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmbeddedControllerZPosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("part_controllers_embed_relative_positions", "pos_z", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("part_controllers_embed_relative_positions", "pos_z");
        }
    }
}
