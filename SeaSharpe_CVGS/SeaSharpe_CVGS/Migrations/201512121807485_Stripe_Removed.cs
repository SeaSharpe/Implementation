namespace SeaSharpe_CVGS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stripe_Removed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Members", "StripeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "StripeID", c => c.Int(nullable: false));
        }
    }
}
