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

        public FriendshipControllerTest()
        {
            
        }

        public FriendshipControllerTest(ApplicationDbContext db)
        {
            this.db = db;
        }

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
            db.Database.Connection.Close();
            //db.Database.Delete();
            //db = null;
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
