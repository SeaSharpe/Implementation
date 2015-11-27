namespace SeaSharpe_CVGS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Autonumbers : DbMigration
    {
        public override void Up()
        {
            Sql("DBCC CHECKIDENT ( 'Employees'  , RESEED, 200000 )");
            Sql("DBCC CHECKIDENT ( 'Members'    , RESEED, 30000000 )");
            Sql("DBCC CHECKIDENT ( 'Events'     , RESEED, 4000000 )");
            Sql("DBCC CHECKIDENT ( 'Addresses'  , RESEED, 500000000 )");
            Sql("DBCC CHECKIDENT ( 'Orders'     , RESEED, 600000000 )");
            Sql("DBCC CHECKIDENT ( 'Games'      , RESEED, 7000000 )");
            Sql("DBCC CHECKIDENT ( 'Platforms'  , RESEED, 800 )");
            Sql("DBCC CHECKIDENT ( 'Categories' , RESEED, 8000 )");
            Sql("DBCC CHECKIDENT ( 'Reviews'    , RESEED, 900000000 )");
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
