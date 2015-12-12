namespace SeaSharpe_CVGS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Schema_Additions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Preferences", c => c.String(maxLength: 500));
            AddColumn("dbo.Games", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Members", "StripeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "StripeID", c => c.Int(nullable: false));
            DropColumn("dbo.Games", "IsActive");
            DropColumn("dbo.Members", "Preferences");
        }
    }
}
