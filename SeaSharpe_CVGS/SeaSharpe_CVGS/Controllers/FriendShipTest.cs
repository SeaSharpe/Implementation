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
    public class FriendShipTest : Controller
    {
        public FriendShipTest()
        {
            Index("");
            //AddFriend("MICHAELDOORLEY145275");
                //THOMASVALASCO422739 //BETHMILLARD721478 //ANNEMANNIX757533 //THOMASWILLIAMS156589
                //BERNARDXAINTONG4274 //SARWATWINSMAN553131

            AddFamily("KAYCHERNY183636");
        }

        //[SetUp]
        public void Init()
        {
            // make connection
            //db = new ApplicationDbContext();

            // seed the database
            //new Migrations.Configuration().SeedDebug(new ApplicationDbContext());
        }

        //[TearDown]
        public void Cleanup()
        {
            //db.Database.Delete();
            //db = null;
        }

        //[TestCase(1)]
        public void Index(string searchName)
        {
            var controller = new FriendshipController();
            Debug.Print(controller.db.Members.Count()+"");
            Member member = controller.DbContext.Members.First();

            Debug.Print(member.Id+" . "+member.User.UserName);
            // Act
            controller.ControllerContext = GetControllerContext(db, member, new string[] {"Member"});
            ViewResult result = controller.Index(searchName) as ViewResult;

            Assert.IsNotNull(result);

            if (result != null)
            {
                Debug.Print("Edit() pass");
            }
        }

        //[Test]
        public void AddFriend(string userName)
        {
            var controller = new FriendshipController();
            Debug.Print(controller.db.Members.Count() + "");
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, new string[] { "Member" });

            var initCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && !a.IsFamilyMember).ToList().Count;

            Debug.Print(initCount + " before");

            // Act
            ViewResult result = controller.AddFriend(userName) as ViewResult;

            var finCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && !a.IsFamilyMember).ToList().Count;
            Debug.Print(finCount + " after");

            // Assert
            Assert.AreEqual(initCount+1, finCount);

            if (initCount+1 == finCount)
            {
                Debug.Print("AddFriend() pass");
            }
        }


        //[Test]
        public void AddFamily(string userName)
        {
            var controller = new FriendshipController();
            Debug.Print(controller.db.Members.Count() + "");
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, new string[] { "Member" });

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

            if (initCount + 1 == finCount)
            {
                Debug.Print("AddFamily() pass");
            }
        }


        //[Test]
        public void Details(int id)
        {
            var controller = new FriendshipController();
            Debug.Print(controller.db.Members.Count() + "");
            Member member = controller.DbContext.Members.First();

            controller.ControllerContext = GetControllerContext(db, member, new string[] { "Member" });

            var initCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && a.IsFamilyMember).ToList().Count;

            // Act
            ViewResult result = controller.Details(id) as ViewResult;

            var finCount = controller.DbContext.Friendships.Where(a => a.Friender.User.Id == member.User.Id
                && a.IsFamilyMember).ToList().Count;

            // Assert
            Assert.AreEqual(initCount + 1, finCount);

            if (initCount + 1 == finCount)
            {
                Debug.Print("AddFamily() pass");
            }
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