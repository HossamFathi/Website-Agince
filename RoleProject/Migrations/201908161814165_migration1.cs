namespace RoleProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Car_propertiesCar", name: "Car_properties_id", newName: "Car_properties");
            RenameColumn(table: "dbo.Car_propertiesCar", name: "Car_Car_Id", newName: "Cars");
            RenameIndex(table: "dbo.Car_propertiesCar", name: "IX_Car_Car_Id", newName: "IX_Cars");
            RenameIndex(table: "dbo.Car_propertiesCar", name: "IX_Car_properties_id", newName: "IX_Car_properties");
            DropPrimaryKey("dbo.Car_propertiesCar");
            AddPrimaryKey("dbo.Car_propertiesCar", new[] { "Cars", "Car_properties" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Car_propertiesCar");
            AddPrimaryKey("dbo.Car_propertiesCar", new[] { "Car_properties_id", "Car_Car_Id" });
            RenameIndex(table: "dbo.Car_propertiesCar", name: "IX_Car_properties", newName: "IX_Car_properties_id");
            RenameIndex(table: "dbo.Car_propertiesCar", name: "IX_Cars", newName: "IX_Car_Car_Id");
            RenameColumn(table: "dbo.Car_propertiesCar", name: "Cars", newName: "Car_Car_Id");
            RenameColumn(table: "dbo.Car_propertiesCar", name: "Car_properties", newName: "Car_properties_id");
        }
    }
}
