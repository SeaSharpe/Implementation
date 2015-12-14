using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SeaSharpe_CVGS.Controllers;
using SeaSharpe_CVGS.Models;
using System.Web.Mvc;
using System.Web;
using System.Security.Principal;
using Moq;
using System.Globalization;
using System.Net;
using System.IO;
using System.Transactions;

namespace SeaSharpe_CVGS.Tests.Controllers
{
    /// <summary>
    /// Class for testing the methods of the review controller
    /// </summary>
    class ReviewControllerTest
    {
        TransactionScope _trans;
        ApplicationDbContext db = null;
        GameController controller;

        [SetUp]
        public void Init()
        {
            _trans = new TransactionScope();
            // make connection
            db = new ApplicationDbContext();

            //make controller
            controller = new GameController { DbContext = db };

            // seed the database if empty
            if (db.Members.Count() == 0)
            {
                new Migrations.Configuration().SeedDebug(db);
                Console.WriteLine("Seeded DB");
            }
            else
            {
                Console.WriteLine("DB Already seeded");
            }

        }

        [TearDown]
        public void Cleanup()
        {
            _trans.Dispose();
            //db.Database.Delete();
        }
    }
}
