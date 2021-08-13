namespace Attanaya_Warrior_Institute.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migraton1 : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GiftCertificateModels");
        }
    }
}
