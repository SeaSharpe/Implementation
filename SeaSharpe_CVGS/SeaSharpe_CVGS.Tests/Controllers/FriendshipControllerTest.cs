using NUnit.Framework;
using SeaSharpe_CVGS.Controllers;
using SeaSharpe_CVGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeaSharpe_CVGS.Tests.Controllers
{
    class FriendshipControllerTest
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

        [Test]
        public void Index()
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.Index("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddFriend()
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFriend("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddFamily()
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFamily("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void MutualFriendShip()
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFamily("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
