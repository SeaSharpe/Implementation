namespace SeaSharpe_CVGS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Autonumbers : DbMigration
    {
        public override void Up()
        {
            Sql("DBCC CHECKIDENT ( 'Employees'  , RESEED, 200001)");
            Sql("DBCC CHECKIDENT ( 'Members'    , RESEED, 30000001)");
            Sql("DBCC CHECKIDENT ( 'Events'     , RESEED, 4000001)");
            Sql("DBCC CHECKIDENT ( 'Addresses'  , RESEED, 500000001)");
            Sql("DBCC CHECKIDENT ( 'Orders'     , RESEED, 600000001)");
            Sql("DBCC CHECKIDENT ( 'Games'      , RESEED, 7000001)");
            Sql("DBCC CHECKIDENT ( 'Platforms'  , RESEED, 801)");
            Sql("DBCC CHECKIDENT ( 'Categories' , RESEED, 8001)");
            Sql("DBCC CHECKIDENT ( 'Reviews'    , RESEED, 900000001)");
        }
        
        public override void Down()
        {
            Sql("DBCC CHECKIDENT ( 'Employees'  , RESEED, 1 )");
            Sql("DBCC CHECKIDENT ( 'Members'    , RESEED, 1 )");
            Sql("DBCC CHECKIDENT ( 'Events'     , RESEED, 1 )");
            Sql("DBCC CHECKIDENT ( 'Addresses'  , RESEED, 1 )");
            Sql("DBCC CHECKIDENT ( 'Orders'     , RESEED, 1 )");
            Sql("DBCC CHECKIDENT ( 'Games'      , RESEED, 1 )");
            Sql("DBCC CHECKIDENT ( 'Platforms'  , RESEED, 1 )");
            Sql("DBCC CHECKIDENT ( 'Categories' , RESEED, 1 )");
            Sql("DBCC CHECKIDENT ( 'Reviews'    , RESEED, 1 )");
        }
    }
}
