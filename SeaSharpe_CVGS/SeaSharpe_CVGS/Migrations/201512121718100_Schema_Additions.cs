namespace SeaSharpe_CVGS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Schema_Additions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Preferences", c => c.String(maxLength: 500));
            AddColumn("dbo.Games", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "IsActive");
            DropColumn("dbo.Members", "Preferences");
        }
    }
}
