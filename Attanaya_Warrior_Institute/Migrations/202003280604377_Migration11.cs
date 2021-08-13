namespace Attanaya_Warrior_Institute.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration11 : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.GiftCertificateModels",
                c => new
                {
                    GiftCertificateNumber = c.Guid(nullable: false),
                    PackageType = c.Int(nullable: false),
                    PurchaserFirstName = c.String(),
                    PurchaserMiddleName = c.String(),
                    PurchaserLastName = c.String(),
                    PurchaserEmailAddress = c.String(),
                    PurchaserPhoneNumber = c.String(),
                    PurchaseDate = c.DateTime(nullable: false),
                    RecipientName = c.String(),
                    RecipientPhone = c.String(),
                    RecipientEmail = c.String(),
                    RedemptionDate = c.DateTime(),
                })
                .PrimaryKey(t => t.GiftCertificateNumber);


            AddColumn("dbo.GiftCertificateModels", "PackageDescription", c => c.String());
            AlterColumn("dbo.GiftCertificateModels", "PurchaserMiddleName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GiftCertificateModels", "PurchaserMiddleName", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.GiftCertificateModels", "PackageDescription");
            DropTable("dbo.GiftCertificateModels");
        }
    }
}
