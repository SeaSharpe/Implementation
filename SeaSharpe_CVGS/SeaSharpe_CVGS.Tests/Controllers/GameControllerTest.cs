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

        [Test]
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

        [Test]
        public void SearchByPlatform()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set platform search string
            string platformString = "PS4";
            List<Game> games = db.Games.Where(g => g.Platform.Name == platformString).ToList();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");


            //Call the controller method
            ActionResult result = controller.SearchBy(platformString, null);

            //Get model from controller action
            var model = ((ViewResult)result).Model as List<Game>;

            //Compare model to expected collection
            CollectionAssert.AreEqual(games, model);
        }

        [Test]
        public void SearchByCategory()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set category search string
            string categoryString = "Action";

            //Get games with matching categories
            List<Game> games = db.Games.Where(g => g.Categories.Where(c => c.Name == categoryString).Count() > 0).ToList();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");


            //Call the controller method
            ActionResult result = controller.SearchBy(null, categoryString);

            //Get model from controller action
            var model = ((ViewResult)result).Model as List<Game>;

            //Compare model to expected collection
            CollectionAssert.AreEqual(games, model);
        }

        [Test]
        public void SearchByCategoryNoResults()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set category search string
            string categoryString = "not a realy category";
            string platformString = "not a realy platform";

            //Get games with matching categories
            List<Game> games = db.Games.Where(g => g.Categories.Where(c => c.Name == categoryString).Count() > 0).ToList();

            //Get games with matching platforms
            games = games.Where(g => g.Platform.Name == platformString).ToList();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");


            //Call the controller method
            ActionResult result = controller.SearchBy(platformString, categoryString);

            //Get model from controller action
            var model = ((ViewResult)result).Model as List<Game>;

            //Compare model to expected collection
            CollectionAssert.AreEqual(games, model);
        }

        [Test]
        public void DetailsExists()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Get games with matching categories
            Game game = db.Games.FirstOrDefault();
            int id = game.Id;

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Call the controller method
            ActionResult result = controller.Details(id);

            //Get model from controller action
            var model = ((ViewResult)result).Model as Game;

            //Compare model to expected collection
            Assert.AreEqual(game, model);
        }

        [Test]
        public void DetailsNotExists()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //invalid game id
            int id = -1;

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Call the controller method
            ActionResult result = controller.Details(id);
            

            //Compare model to expected collection
            Assert.IsInstanceOf(new HttpNotFoundResult().GetType(), result);
        }

        [Test]
        public void DetailsNullId()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Call the controller method
            HttpStatusCodeResult result = (HttpStatusCodeResult)controller.Details(null);
            HttpStatusCodeResult badRequest = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Compare model to expected collection
            Assert.IsInstanceOf(badRequest.GetType(), result);
        }

        [Test]
        public void Download()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");
            
            //Get angry birds game
            Game game = db.Games.Where(g => g.Name == "Angry Birds: Star Wars").First();
            //Call the controller method
            var result = controller.Download(game.Id); 

        }
    }

}
