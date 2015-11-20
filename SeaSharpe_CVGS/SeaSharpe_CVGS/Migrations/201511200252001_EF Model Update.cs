namespace SeaSharpe_CVGS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EFModelUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventMembers", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.EventMembers", "Member_Id", "dbo.Members");
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
                .ForeignKey("dbo.Members", t => t.Member_Id);
            
            AddForeignKey("dbo.GameCategories", "Game_Id", "dbo.Games", "Id");
            AddForeignKey("dbo.GameCategories", "Category_Id", "dbo.Categories", "Id");
            DropTable("dbo.EventMembers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EventMembers",
                c => new
                    {
                        Member_Id = c.Int(nullable: false),
                        Event_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Member_Id, t.Event_Id });
            
            DropForeignKey("dbo.GameCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.GameCategories", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.EventMembers", "Member_Id", "dbo.Members");
            DropForeignKey("dbo.EventMembers", "Event_Id", "dbo.Events");
            DropTable("dbo.EventMembers");
            AddForeignKey("dbo.GameCategories", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GameCategories", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EventMembers", "Member_Id", "dbo.Members", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EventMembers", "Event_Id", "dbo.Events", "Id", cascadeDelete: true);
        }
    }
}
