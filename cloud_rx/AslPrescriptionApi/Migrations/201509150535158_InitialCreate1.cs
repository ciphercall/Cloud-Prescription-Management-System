namespace AslPrescriptionApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RX_PRESCMST", "AMOUNT", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RX_PRESCMST", "AMOUNT");
        }
    }
}
