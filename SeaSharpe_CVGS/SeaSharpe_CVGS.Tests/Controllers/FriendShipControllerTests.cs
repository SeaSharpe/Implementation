using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using SeaSharpe_CVGS.Models;
using NUnit.Framework;
using Moq;


namespace SeaSharpe_CVGS.Controllers
{
    public class FriendShipControllerTests 
    {
        public FriendShipControllerTests()
        {
            //Index("");
            //AddFriend("MICHAELDOORLEY145275");
            //THOMASVALASCO422739 //BETHMILLARD721478 //ANNEMANNIX757533 //THOMASWILLIAMS156589
            //BERNARDXAINTONG4274 //SARWATWINSMAN553131

            //AddFamily("KAYCHERNY183636");
            //Details(30000015);
            //Delete(30000015);
            //AddToWishList(7000001);
            //RemoveFromWishlist(7000001);
            //MoveToCart(7000001);
        }

        ApplicationDbContext db = null;

        [SetUp]
        public void Init()
        {
            // make connection
            db = new ApplicationDbContext();

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
            //db.Database.Delete();
        }

        [TestCase("")]
        public void Index(string searchName)
        {
            var controller = new FriendshipController();
            Debug.Print(controller.db.Members.Count() + "");
            Member member = controller.DbContext.Members.First();

            Debug.Print(member.Id + " . " + member.User.UserName);
            // Act
            controller.ControllerContext = GetControllerContext(db, member, "Member");
            ViewResult result = controller.Index(searchName) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestCase("MICHAELDOORLEY145275")]
        public void AddFriend(string userName)
        {
            var controller = new FriendshipController();
            Debug.Print(controller.db.Members.Count() + "");
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            var initCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && !a.IsFamilyMember).ToList().Count;

            Debug.Print(initCount + " before");

            // Act
            ViewResult result = controller.AddFriend(userName) as ViewResult;

            var finCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && !a.IsFamilyMember).ToList().Count;
            Debug.Print(finCount + " after");

            // Assert
            Assert.AreEqual(initCount + 1, finCount);
        }


        [TestCase("KAYCHERNY183636")]
        public void AddFamily(string userName)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            var initCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && a.IsFamilyMember).ToList().Count;

            Debug.Print(initCount + " before");

            // Act
            ViewResult result = controller.AddFamily(userName) as ViewResult;

            var finCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && a.IsFamilyMember).ToList().Count;
            Debug.Print(finCount + " after");

            // Assert
            Assert.AreEqual(initCount + 1, finCount);
        }


        [TestCase(30000015)]
        public void Details(int id)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            // Act
            ViewResult result = controller.Details(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }


        [TestCase(30000015)]
        public void Delete(int id)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            var initCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id).ToList().Count;

            Debug.Print(initCount + " before");

            // Act
            ViewResult result = controller.Delete(id) as ViewResult;

            var finCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id).ToList().Count;
            Debug.Print(finCount + " after");

            // Assert
            Assert.AreEqual(initCount - 1, finCount);
        }

        [TestCase(7000001)]
        void MoveToCart(int gameId)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            var initCount = controller.DbContext.Orders.Where(a => a.Member.Id == member.Id).ToList().Count;
            Debug.Print(initCount + " before");

            // Act
            ViewResult result = controller.MoveToCart(gameId, member.Id) as ViewResult;

            var finCount = controller.DbContext.Orders.Where(a => a.Member.Id == member.Id).ToList().Count;
            Debug.Print(finCount + " after");

            // Assert
            Assert.AreEqual(initCount + 1, finCount);
        }

        [TestCase(7000001)]
        void RemoveFromWishlist(int id)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            var initCount = controller.DbContext.WishLists.Where(w => w.MemberId == member.Id && w.GameId == id).ToList().Count;

            Debug.Print(initCount + " before");

            // Act
            ViewResult result = controller.RemoveFromWishlist(id, member.Id) as ViewResult;

            var finCount = controller.DbContext.WishLists.Where(w => w.MemberId == member.Id && w.GameId == id).ToList().Count;
            Debug.Print(finCount + " after");

            // Assert
            Assert.AreEqual(initCount - 1, finCount);
        }

        [TestCase(7000001)]
        public void AddToWishList(int id)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            var initCount = controller.DbContext.WishLists.Where(w => w.MemberId == member.Id && w.GameId == id).ToList().Count;

            Debug.Print(initCount + " before");

            // Act
            ViewResult result = controller.AddToWishList(id) as ViewResult;

            var finCount = controller.DbContext.WishLists.Where(w => w.MemberId == member.Id && w.GameId == id).ToList().Count;
            Debug.Print(finCount + " after");

            // Assert
            Assert.AreEqual(initCount + 1, finCount);
        }




        ControllerContext GetControllerContext(ApplicationDbContext db, Member member, params string[] roles)
        {
            var userMock = new Mock<IPrincipal>();

            // Return true for "member" and "Member" roles
            foreach (string role in roles)
            {
                userMock.Setup(p => p.IsInRole(role)).Returns(true);
            }

            // Return first username
            userMock.Setup(p => p.Identity.Name).Returns(member.User.UserName);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            return controllerContextMock.Object;
        }
    }
}