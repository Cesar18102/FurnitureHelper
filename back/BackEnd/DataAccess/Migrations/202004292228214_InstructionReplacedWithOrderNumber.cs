namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstructionReplacedWithOrderNumber : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("furniture_item_instructions", "global_connection_id", "furniture_item_parts_connections");
            DropIndex("furniture_item_instructions", new[] { "global_connection_id" });
            DropTable("furniture_item_instructions");

            AddColumn("furniture_item_parts_connections", "order_number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            CreateTable(
                "furniture_item_instructions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        global_connection_id = c.Int(nullable: false),
                        global_connection_order_number = c.Int(nullable: false),
                        step_text = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id);

            CreateIndex("furniture_item_instructions", "global_connection_id");
            AddForeignKey("furniture_item_instructions", "global_connection_id", "furniture_item_parts_connections", "id", cascadeDelete: true);

            DropColumn("furniture_item_parts_connections", "order_number");
        }
    }
}
