namespace AslPrescriptionApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ASL_UPLOAD_GCONTACT",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        GROUPID = c.Long(nullable: false),
                        NAME = c.String(),
                        EMAIL = c.String(),
                        MOBILENO = c.String(),
                        ADDRESS = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID });
            
            CreateTable(
                "dbo.ASL_UPLOAD_GROUP",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        GROUPID = c.Long(nullable: false),
                        GROUPNM = c.String(),
                        USERPC = c.String(),
                        INSUSERID = c.Long(nullable: false),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(nullable: false),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.GROUPID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ASL_UPLOAD_GROUP");
            DropTable("dbo.ASL_UPLOAD_GCONTACT");
        }
    }
}
