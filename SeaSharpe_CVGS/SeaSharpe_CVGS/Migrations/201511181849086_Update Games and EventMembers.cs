namespace SeaSharpe_CVGS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGamesandEventMembers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameCategories", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.GameCategories", "Category_Id", "dbo.Categories");
            CreateTable(
                "dbo.EventMembers",
                c => new
                    {
                        Event_Id = c.Int(nullable: false),
                        Member_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Member_Id })
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.Members", t => t.Member_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.Member_Id);
            
            AddColumn("dbo.Games", "ImagePath", c => c.String());
            AddColumn("dbo.Games", "Publisher", c => c.String(maxLength: 50));
            AddColumn("dbo.Games", "ESRB", c => c.String(maxLength: 4));
            AddForeignKey("dbo.GameCategories", "Game_Id", "dbo.Games", "Id");
            AddForeignKey("dbo.GameCategories", "Category_Id", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.GameCategories", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.EventMembers", "Member_Id", "dbo.Members");
            DropForeignKey("dbo.EventMembers", "Event_Id", "dbo.Events");
            DropIndex("dbo.EventMembers", new[] { "Member_Id" });
            DropIndex("dbo.EventMembers", new[] { "Event_Id" });
            DropColumn("dbo.Games", "ESRB");
            DropColumn("dbo.Games", "Publisher");
            DropColumn("dbo.Games", "ImagePath");
            DropTable("dbo.EventMembers");
            AddForeignKey("dbo.GameCategories", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GameCategories", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
        }
    }
}
