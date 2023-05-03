namespace AslPrescriptionApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ASL_COMPANY", "EMAILIDP", c => c.String());
            AddColumn("dbo.ASL_COMPANY", "EMAILPWP", c => c.String());
            AddColumn("dbo.ASL_COMPANY", "SMSIDP", c => c.String());
            AddColumn("dbo.ASL_COMPANY", "SMSPWP", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ASL_COMPANY", "SMSPWP");
            DropColumn("dbo.ASL_COMPANY", "SMSIDP");
            DropColumn("dbo.ASL_COMPANY", "EMAILPWP");
            DropColumn("dbo.ASL_COMPANY", "EMAILIDP");
        }
    }
}
