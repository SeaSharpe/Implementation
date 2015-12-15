using NUnit.Framework;
using SeaSharpe_CVGS.Controllers;
using SeaSharpe_CVGS.Models;
using System;
using System.Linq;
using System.Web.Mvc;

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
            db.Database.Connection.Close();
            //db.Database.Delete();
            //db = null;
        }

        [TestCase("nana")]
        [TestCase("lee")]
        public void Index(string nameSearch)
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.Index(nameSearch) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddFriend(string userName)
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFriend("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddFamily(string userName)
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFamily("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Details(int id)
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFamily("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }        
        
        [Test]
        public void Delete(int id)
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFamily("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }     
        
        [Test]
        public void MoveToCart(int gameId, int memberId)
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFamily("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void RemoveFromWishlist(int gameId, int memberId)
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFamily("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddToWishList(int id)
        {
            FriendshipController controller = new FriendshipController();

            // Act
            ViewResult result = controller.AddFamily("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //helper methods
        [Test]
        public void DisplayCurrentMemberWishlist()
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
