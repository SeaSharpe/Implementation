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

namespace SeaSharpe_CVGS.Tests.Controllers
{
    class GameControllerTest
    {

        ApplicationDbContext db = null;
        GameController controller;

        [SetUp]
        public void Init()
        {
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
            //db.Database.Delete();
        }

        [Test]
        //Test game index for employee
        public void EmployeeIndex()
        {
            //Get employee from db
            Employee employee = db.Employees.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee,"Employee");

            //Call the controller method
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Index();

            //Check if action being redirected to is GameManagement
            Assert.AreEqual("GameManagement", result.RouteValues["action"]);
        }

        [Test]
        //Test game index for member
        public void MemberIndex()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Call the controller method
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Index();

            //Check if action being redirected to is GameManagement
            Assert.AreEqual("SearchGames", result.RouteValues["action"]);
        }
        
        [Test]
        //Test game index for visitor
        public void VisitorIndex()
        {            

            //Call the controller method
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Index();

            //Check if action being redirected to is GameManagement
            Assert.AreEqual("SearchGames", result.RouteValues["action"]);
        }

        [TestCase]
        public void SearchGamesName()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set name search string
            String nameSearch = "Just Cause 3";

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Get the games list
            List<Game> games = db.Games.Where(g => g.Name.Contains(nameSearch)).ToList();

            //Call the controller method
            ActionResult result = controller.SearchGames(nameSearch, null, null, null, false);

            //Get model from controller action
            var model = ((ViewResult)result).Model as List<Game>;

            //Compare model to expected collection
            CollectionAssert.AreEqual(games, model);

        }

        [TestCase]
        public void SearchGamesESRB()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set name search string
            String esrbCode = "T";

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Get the games list
            List<Game> games = db.Games.Where(g => g.ESRB.Contains(esrbCode)).ToList();

            //Call the controller method
            ActionResult result = controller.SearchGames(null, null, null, new string[] {esrbCode}, false);

            //Get model from controller action
            var model = ((ViewResult)result).Model as List<Game>;

            //Compare model to expected collection
            CollectionAssert.AreEqual(games, model);
        }

        [TestCase]
        public void SearchGamesPlatform()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set name search string
            int[] platformIds = new int[] { 801, 802 };

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Get the games list
            List<Game> games = db.Games.Where(g => platformIds.Contains(g.Platform_Id)).ToList();

            //Call the controller method
            ActionResult result = controller.SearchGames(null, platformIds, null, null, false);

            //Get model from controller action
            var model = ((ViewResult)result).Model as List<Game>;

            //Compare model to expected collection
            CollectionAssert.AreEqual(games, model);
        }

        [TestCase]
        public void SearchGamesCategoryExlusive()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set name search string
            int[] categoryIds = new int[] { 8001, 8002 };
            ICollection<Category> categories = db.Catagories.Where(c => categoryIds.Contains(c.Id)).ToList();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Get the games list
            List<Game> games = db.Games.Include(g => g.Categories).ToList();
            games = games.Where(g => g.Categories.Intersect(categories).Any()).ToList();

            //Call the controller method
            ActionResult result = controller.SearchGames(null, null, categoryIds, null, false);

            //Get model from controller action
            var model = ((ViewResult)result).Model as List<Game>;

            //Compare model to expected collection
            CollectionAssert.AreEqual(games, model);
        }

        [TestCase]
        public void SearchGamesCategoryInclusive()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set name search string
            int[] categoryIds = new int[] { 8001, 8002 };
            ICollection<Category> categories = db.Catagories.Where(c => categoryIds.Contains(c.Id)).ToList();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Get the games list
            List<Game> games = db.Games.Include(g => g.Categories).ToList();
            games = games.Where(g => g.Categories.Intersect(categories).Count() == categories.Count).ToList();

            //Call the controller method
            ActionResult result = controller.SearchGames(null, null, categoryIds, null, true);

            //Get model from controller action
            var model = ((ViewResult)result).Model as List<Game>;

            //Compare model to expected collection
            CollectionAssert.AreEqual(games, model);
        }
    }

}
