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

namespace SeaSharpe_CVGS.Tests.Controllers
{
    class UserControllerTests
    {
        ApplicationDbContext db = null;

        [SetUp]
        public void Init()
        {
            // make connection
            db = new ApplicationDbContext(); 

            // seed the database
            new Migrations.Configuration().SeedDebug(new ApplicationDbContext());
        }

        [TearDown]
        public void Cleanup()
        {
            db.Database.Delete();
            db = null;
        }

        [TestCase(1)]
        public void Edit(int memberIndex)
        {
            // Arrange
            var controller = new UserController();
            Console.WriteLine(controller.db.Members.Count());
            Member member = controller.DbContext.Members.First();
            Console.WriteLine(member.Id);
            // Act
            ViewResult result = controller.Edit(member.Id) as ViewResult;

            //var newContext = new ApplicationDbContext();
            
            Assert.IsNotNull(result);
        }
    }
}
