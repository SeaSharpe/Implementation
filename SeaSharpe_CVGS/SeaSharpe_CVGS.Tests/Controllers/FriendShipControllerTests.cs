/*
 * File Name: FriendshipControllerTests.cs
 *  
 * Revision History:
 *      17-Dec-2015: Manuel Lopez. Wrote code
 */

using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using SeaSharpe_CVGS.Models;
using NUnit.Framework;
using Moq;
using System.Transactions;

namespace SeaSharpe_CVGS.Controllers
{
    public class FriendShipControllerTests 
    {
        TransactionScope _trans;
        ApplicationDbContext db = null;

        [SetUp]
        public void Init()
        {
            _trans = new TransactionScope();

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
            _trans.Dispose();
        }

        /// <summary>
        /// Test case of searc a name
        /// </summary>
        /// <param name="searchName"></param>
        [TestCase("")]
        public void Index(string searchName)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            // Act
            controller.ControllerContext = GetControllerContext(db, member, "Member");
            ViewResult result = controller.Index(searchName) as ViewResult;

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test case of add a friend
        /// </summary>
        /// <param name="userName"></param>
        [TestCase("THOMASVALASCO422739")]
        public void AddFriend(string userName)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            var initCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && !a.IsFamilyMember).ToList().Count;

            // Act
            ViewResult result = controller.AddFriend(userName) as ViewResult;

            var finCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && !a.IsFamilyMember).ToList().Count;

            // Assert
            Assert.AreEqual(initCount + 1, finCount);
        }

        /// <summary>
        /// Test case of adding a family
        /// </summary>
        /// <param name="userName"></param>
        [TestCase("BETHMILLARD721478")]
        public void AddFamily(string userName)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            var initCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && a.IsFamilyMember).ToList().Count;

            // Act
            ViewResult result = controller.AddFamily(userName) as ViewResult;

            var finCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && a.IsFamilyMember).ToList().Count;

            // Assert
            Assert.AreEqual(initCount + 1, finCount);
        }

        /// <summary>
        /// Testcase to get the details of a friend 
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// Deleting a frienship
        /// </summary>
        /// <param name="id"></param>
        [TestCase(30000019)]
        public void Delete(int id)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");
            controller.AddFamily("SARWATWINSMAN553131");
            var initCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id).ToList().Count;

            // Act
            ViewResult result = controller.Delete(id) as ViewResult;

            var finCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id).ToList().Count;

            // Assert
            Assert.AreEqual(initCount - 1, finCount);
        }

        /// <summary>
        /// Moving a game to cart
        /// </summary>
        /// <param name="gameId"></param>
        [TestCase(7000001)]
        public void MoveToCart(int gameId)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();
            controller.ControllerContext = GetControllerContext(db, member, "Member");

            // Act
            ViewResult result = controller.MoveToCart(gameId, member.Id) as ViewResult;

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Remove a game from wishlist
        /// </summary>
        /// <param name="id"></param>
        [TestCase(7000001)]
        public void RemoveFromWishlist(int id)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            //Adding it first 
            controller.AddToWishList(id);
            var initCount = controller.DbContext.WishLists.Where(w => w.MemberId == member.Id && w.GameId == id).ToList().Count;
            
            // Act
            ViewResult result = controller.RemoveFromWishlist(id, member.Id) as ViewResult;

            var finCount = controller.DbContext.WishLists.Where(w => w.MemberId == member.Id && w.GameId == id).ToList().Count;

            // Assert
            Assert.AreEqual(initCount - 1, finCount);
        }

        /// <summary>
        /// Add a game to wishlist
        /// </summary>
        /// <param name="id"></param>
        [TestCase(7000001)]
        public void AddToWishList(int id)
        {
            var controller = new FriendshipController();
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, "Member");

            var initCount = controller.DbContext.WishLists.Where(w => w.MemberId == member.Id && w.GameId == id).ToList().Count;

            // Act
            ViewResult result = controller.AddToWishList(id) as ViewResult;
            var finCount = controller.DbContext.WishLists.Where(w => w.MemberId == member.Id && w.GameId == id).ToList().Count;

            // Assert
            Assert.AreEqual(initCount + 1, finCount);
        }

        /// <summary>
        /// Utility
        /// </summary>
        /// <param name="db"></param>
        /// <param name="member"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
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