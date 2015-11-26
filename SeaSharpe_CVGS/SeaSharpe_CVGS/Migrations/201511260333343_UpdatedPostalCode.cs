namespace SeaSharpe_CVGS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPostalCode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "PostalCode", c => c.String(nullable: false, maxLength: 6, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "PostalCode", c => c.String(nullable: false, maxLength: 5, fixedLength: true, unicode: false));
        }
    }
}
