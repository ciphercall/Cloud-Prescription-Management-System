namespace AslPrescriptionApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ASL_PEMAIL",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TRANSYY = c.Long(nullable: false),
                        TRANSNO = c.Long(nullable: false),
                        TRANSDT = c.DateTime(),
                        EMAILID = c.String(maxLength: 100),
                        EMAILSUBJECT = c.String(),
                        BODYMSG = c.String(),
                        STATUS = c.String(maxLength: 7),
                        SENTTM = c.DateTime(),
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
                .PrimaryKey(t => new { t.ID, t.COMPID, t.TRANSYY, t.TRANSNO });
            
            CreateTable(
                "dbo.ASL_PSMS",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TRANSYY = c.Long(nullable: false),
                        TRANSNO = c.Long(nullable: false),
                        TRANSDT = c.DateTime(),
                        MOBNO = c.String(maxLength: 13),
                        LANGUAGE = c.String(maxLength: 3),
                        MESSAGE = c.String(maxLength: 160),
                        STATUS = c.String(maxLength: 7),
                        SENTTM = c.DateTime(),
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
                .PrimaryKey(t => new { t.ID, t.COMPID, t.TRANSYY, t.TRANSNO });
            
            CreateTable(
                "dbo.ASL_PCONTACTS",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        GROUPID = c.Long(nullable: false),
                        NAME = c.String(),
                        EMAIL = c.String(),
                        MOBNO1 = c.String(),
                        MOBNO2 = c.String(),
                        ADDRESS = c.String(),
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
                .PrimaryKey(t => new { t.ID, t.COMPID });
            
            CreateTable(
                "dbo.ASL_PGROUPS",
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
            
            AddColumn("dbo.ASL_COMPANY", "ADDRESS2", c => c.String());
            DropTable("dbo.ASL_UPLOAD_GCONTACT");
            DropTable("dbo.ASL_UPLOAD_GROUP");
        }
        
        public override void Down()
        {
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
            
            DropColumn("dbo.ASL_COMPANY", "ADDRESS2");
            DropTable("dbo.ASL_PGROUPS");
            DropTable("dbo.ASL_PCONTACTS");
            DropTable("dbo.ASL_PSMS");
            DropTable("dbo.ASL_PEMAIL");
        }
    }
}
