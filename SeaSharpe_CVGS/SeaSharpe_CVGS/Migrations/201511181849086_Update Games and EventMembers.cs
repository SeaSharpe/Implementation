namespace SeaSharpe_CVGS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGamesandEventMembers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventMembers",
                c => new
                    {
                        Member_Id = c.Int(nullable: false),
                        Event_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Member_Id, t.Event_Id })
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.Member_Id)
                .Index(t => t.Member_Id)
                .Index(t => t.Event_Id);
            
            AddColumn("dbo.Games", "ImagePath", c => c.String());
            AddColumn("dbo.Games", "Publisher", c => c.String(maxLength: 50));
            AddColumn("dbo.Games", "ESRB", c => c.String(maxLength: 4));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventMembers", "Member_Id", "dbo.Members");
            DropForeignKey("dbo.EventMembers", "Event_Id", "dbo.Events");
            DropIndex("dbo.EventMembers", new[] { "Event_Id" });
            DropIndex("dbo.EventMembers", new[] { "Member_Id" });
            DropColumn("dbo.Games", "ESRB");
            DropColumn("dbo.Games", "Publisher");
            DropColumn("dbo.Games", "ImagePath");
            DropTable("dbo.EventMembers");
        }
    }
}
