namespace Attanaya_Warrior_Institute.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GiftCertificateModels", "PurchaserFirstName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.GiftCertificateModels", "PurchaserMiddleName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.GiftCertificateModels", "PurchaserLastName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.GiftCertificateModels", "PurchaserEmailAddress", c => c.String(nullable: false));
            AlterColumn("dbo.GiftCertificateModels", "PurchaserPhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.GiftCertificateModels", "RecipientName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.GiftCertificateModels", "RecipientPhone", c => c.String(nullable: false));
            AlterColumn("dbo.GiftCertificateModels", "RecipientEmail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GiftCertificateModels", "RecipientEmail", c => c.String());
            AlterColumn("dbo.GiftCertificateModels", "RecipientPhone", c => c.String());
            AlterColumn("dbo.GiftCertificateModels", "RecipientName", c => c.String());
            AlterColumn("dbo.GiftCertificateModels", "PurchaserPhoneNumber", c => c.String());
            AlterColumn("dbo.GiftCertificateModels", "PurchaserEmailAddress", c => c.String());
            AlterColumn("dbo.GiftCertificateModels", "PurchaserLastName", c => c.String());
            AlterColumn("dbo.GiftCertificateModels", "PurchaserMiddleName", c => c.String());
            AlterColumn("dbo.GiftCertificateModels", "PurchaserFirstName", c => c.String());
        }
    }
}
