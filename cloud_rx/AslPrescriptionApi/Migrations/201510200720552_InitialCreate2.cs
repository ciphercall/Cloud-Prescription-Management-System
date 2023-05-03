namespace AslPrescriptionApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RX_GHEAD", "SHOWTP", c => c.String(maxLength: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RX_GHEAD", "SHOWTP");
        }
    }
}
