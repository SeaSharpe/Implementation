using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeaSharpe_CVGS.Models;
using System.Web.Security;
using System.Collections.Generic;
using System;
using System.IO;

namespace SeaSharpe_CVGS.Controllers
{
    /// <summary>
    /// Controller class for handling actions realted to the Game entity
    /// </summary>
    public class GameController : Controller
    {
        /// <summary>
        /// Struct for holding ESRB rating information 
        /// </summary>
        private struct ESRB
        {
            private string abbreviation;
            public string Abbreviation{get { return abbreviation; } set { abbreviation = value; }}

            private string rating;
            public string Rating{get { return rating; }set { rating = value; }}

            private int minAge;
            public int MinAge{get { return minAge; }set { minAge = value; }}

            public ESRB( string abbreviation,string rating, int minAge)
            {                
                this.abbreviation = abbreviation;
                this.rating = rating;
                this.minAge = minAge;
            }
        }

        //List containing esrb ratings
        private List<ESRB> esrbList = new List<ESRB>()
        {
            new ESRB("EC","Early Childhood",0),
            new ESRB("E", "Everyone", 0),
            new ESRB("E10", "Everyone 10+", 0),
            new ESRB("T", "Teen", 0),
            new ESRB("M", "Mature", 17),
            new ESRB("AO", "Adult Only", 18)
        };
        
        #region Multiple Roles

        /// <summary>
        /// Employee side - list all games
        /// Member side - search feature, list matching games
        /// </summary>
        /// <returns>list of games view</returns>
        public ActionResult Index()
        {
            //User is employee, go to game management page
            if (IsEmployee)
            {
                return RedirectToAction("GameManagement");
            }


            //User is visitor or member, redirect to SearchGames
            else
            {
               return RedirectToAction("SearchGames");
            }
        }

        /// <summary>
        /// Displays game list page for members/visitors
        /// </summary>
        /// <param name="nameSearch">Search fragment for game name</param>
        /// <param name="platformSearch">Platform id for search filtering</param>
        /// <param name="categorySearch">Array of category ids for search filtering</param>
        /// <param name="esrbSearch">ESRB code for search filtering</param>
        /// <param name="isInclusive"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SearchGames(string nameSearch, int[] platformSearch, int[] categorySearch, string[] esrbSearch, bool isInclusive = false)
        {           
            IEnumerable<Game> gameList = db.Games.Include(g => g.Platform).Include(g => g.Categories);
            //Name search query
            if(nameSearch != null)
            {
                gameList = gameList.Where(g => g.Name.ToLower().Contains(nameSearch.ToLower()));
            }

            //Platform search query
            if(platformSearch != null)
            {
                gameList = gameList.Where(g => platformSearch.Contains(g.Platform.Id));
            }

            //Category search query
            if (categorySearch != null)
            {
                ICollection<Category> selectedCategories = db.Catagories.Where(c => categorySearch.Contains(c.Id)).ToList();
                //Only returns rows if game has all selected categories 
                if(isInclusive)
                {
                    gameList = gameList.Where(g => g.Categories.Intersect(selectedCategories).Count() == selectedCategories.Count);
                }

                //returns row if game has any of the selected categories
                else
                {
                    gameList = gameList = gameList.Where(g => g.Categories.Intersect(selectedCategories).Any());
                }
                
            }

            //Esrb search query
            if (esrbSearch != null)
            {                
                gameList = gameList.Where(g => esrbSearch.Contains(g.ESRB));
            }
            
            PopulateDropdownData();
            return View(gameList.ToList());
        }

        /// <summary>
        /// Returns the searchGames action with the specified platform or category
        /// </summary>
        /// <param name="platformStrings">Platform names to search for</param>
        /// <param name="categoryStrings">Category names to search for</param>
        /// <returns></returns>
        public ActionResult SearchBy(string platformString, string categoryString)
        {
            List<Game> gameList = new List<Game>();

            //Get the ids for the platform strings
            if(platformString != null)
            {
                gameList= db.Games.Include(g => g.Platform).Where(g=> platformString.ToLower() == g.Platform.Name.ToLower()).ToList();                
            }

            //Populate categories
            if(categoryString != null)
            {
                gameList = db.Games.Include(g => g.Categories).Where(g => g.Categories.Any(c => c.Name == categoryString.ToLower())).ToList();   
            }

            //Return the search games action with the categories/platforms
            PopulateDropdownData();
            return View("SearchGames",gameList);
        }

        /// <summary>
        /// Get Single Game
        /// **view shared with PartialCreateReview***
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>game details view</returns>
        public ActionResult Details(int? id)
        {                   
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }

            //Initialize age
            int userAge = 0;
            bool ageUndefined = false;

            //ESRB struct for the current games esrb rating
            ESRB esrbRating = esrbList.FirstOrDefault(e => e.Abbreviation == game.ESRB);
            
            //Get members age if user logged in
            if(IsEmployee || IsMember)
            {
                TimeSpan ageDifference = DateTime.Now.Subtract(CurrentUser.DateOfBirth);
                userAge = ageDifference.Days / 365;
            }

            //Check age if user is visitor
            else
            {
                //Check for age cookie and parse into userAge
                HttpCookie ageCookie = Request.Cookies["ageCookie"];
                if (ageCookie != null)
                {
                    int.TryParse(ageCookie.Value, out userAge);
                }
                
                //If the age is still 0(default) after checking cookie, set variables to prompt user in view
                if(userAge == 0)
                {
                    ageUndefined = true;
                    ViewData["minAge"] = esrbRating.MinAge;
                }
                
            }               

            //set viewdata so the javascript prompt is called on the view as needed
            ViewData["ageUndefined"] = ageUndefined;

            //Redirect to search games if user not old enough
            if(userAge < esrbRating.MinAge && !ageUndefined)
            {
                TempData["message"] = "You must be " + esrbRating.MinAge + " years old to view the game: " + game.Name;
                return RedirectToAction("SearchGames");
            }

            //Get tempData message from postback
            TempData["message"] = TempData["message"];

            //Check for review in tempdata from postback 
            Review gameReview = (Review)TempData["review"];

            //Get gameReview if it was not stored in tempdata from postback
            if(gameReview == null)
            {
                if(IsMember)
                {
                    //Get current review for this member/game combination
                    gameReview = db.Reviews.FirstOrDefault(r => r.Author.Id == CurrentMember.Id && r.Game_Id == id);
                }                

                //No review for this user, display blank form
                if (gameReview == null)
                {
                    gameReview = new Review();
                }               

                //Set gameReview game id to current game
                gameReview.Game_Id = game.Id;            
            }

            //Push game review to view so it can be passed to the partial view for review
            ViewData["isApproved"] = gameReview.IsApproved;
            ViewData["review"] = gameReview;          
                   
            return View(game);
        }

        /// <summary>
        /// Downloads the demo for a game, game details page only provides link if the game exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult Download(int id)
        {
            Game game = db.Games.Find(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(@Server.MapPath("/App_Data/" + game.Id + ".zip"));
            string fileName = game.Name + ".zip";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Zip, fileName);
        }

        #endregion

        #region Employee Side

        /// <summary>
        /// Displays game list page for employees
        /// </summary>
        /// <returns>List of games view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult GameManagement()
        {
            IEnumerable<Game> gameList = db.Games.Include(g => g.Platform).Include(g => g.Categories);
            return View(gameList.ToList());
        }

        /// <summary>
        /// Employee Side - Add a game
        /// </summary>
        /// <returns>return add/edit game view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult Create()
        {
            PopulateDropdownData();
            return View();
        }

        /// <summary>
        /// Employee Side - post back for game creation, attempt to save to db
        /// </summary>
        /// <param name="game">game object</param>
        /// <param name="Categories">Array of selected category ids</param>
        /// <param name="Platform">PlatformId</param>
        /// <returns>view of games list</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public ActionResult Create([Bind(Include = "Id,Name,ReleaseDate,SuggestedRetailPrice, ImagePath, Publisher, ESRB")] Game game, int? Platform, int[] Categories)
        {            
            try
            {
                //Add game platform if value not null
                if(Platform != null)
                {
                    Platform gamePlatform = db.Platforms.Find(Platform);
                    game.Platform = gamePlatform;
                }
                
                //Add game categories if value not null
                if(Categories != null)
                {
                    ICollection<Category> gameCategories = (ICollection<Category>)db.Catagories.Where(c => Categories.Contains(c.Id)).ToList();
                    game.Categories = gameCategories;
                }     

                //Update the model state to reflect manual addition of platforms and categories
                ModelState.Clear();
                TryValidateModel(game);

                if (ModelState.IsValid)
                {
                    db.Games.Add(game);
                    db.SaveChanges();
                    return RedirectToAction("GameManagement");
                }
            }

            //Return message to employee if exception
            catch(Exception e)
            {
                TempData["message"] = "Error creating game: " + e.GetBaseException().Message;                
            }

            Create();
            return View(game);
        }

        /// <summary>
        /// Employee side - edit game
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>edit game view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult Edit(int? id)
        {
            //Attempt to access edit page without game id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Game game = db.Games.Find(id);

            //Game with current id does not exist
            if (game == null)
            {
                return HttpNotFound();
            }

            //Selected values for game categories on edit view
            ViewData["Categories"] = game.Categories.Select(c => c.Id);

            //Select value for game platform on edit view
            ViewData["Platform"] = game.Platform.Id;

            PopulateDropdownData();
            return View(game);
        }

        /// <summary>
        /// Employee side - post back for edit game, save to db
        /// </summary>
        /// <param name="game">game object</param>
        /// <param name="Categories">array of categories</param>
        /// <returns>list of games view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public ActionResult Edit([Bind(Include = "Id,Name,ReleaseDate,SuggestedRetailPrice, ImagePath, Publisher, ESRB, Categories, Platform_id")] Game game, int[] Categories)
        {
            try
            {
                //Add platform object for model state
                game.Platform = db.Platforms.Find(game.Platform_Id);

                //Update game categories
                if(Categories == null) Categories = new int[] {};
                ICollection<Category> gameCategories = (ICollection<Category>)db.Catagories.Where(c => Categories.Contains(c.Id)).ToList();
                Game originalGame = db.Games.Find(game.Id);
                originalGame.Categories.Clear();

                foreach (Category c in gameCategories)
                {
                    originalGame.Categories.Add(c);
                }

                db.SaveChanges();
                db.Entry(originalGame).State = EntityState.Detached;

                //Update the model to include binded changes
                ModelState.Clear();
                TryValidateModel(game);

                if (ModelState.IsValid)
                {
                    db.Entry(game).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = "Game with ID: " + game.Id + " updated.";
                    return RedirectToAction("GameManagement");
                }
            }

            catch (Exception e)
            {
                TempData["message"] = e.GetBaseException().Message;
            }            

            Edit(game.Id);
            return View(game);
        }

        /// <summary>
        /// Employee makes the game inactive or active 
        /// ****No view required****
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>list of games view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult Activate(int id)
        {
            Game game = db.Games.Find(id);
            try
            {
                //Change the activity of the game
                game.IsActive = !game.IsActive;

                TryValidateModel(game);

                //Remove game and save changes
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = game.Name + " has had it's activity state changed.";
            }

            catch (Exception e)
            {
                TempData["message"] = "Error changing activity state of game: " + e.GetBaseException().Message;
            }
            
            return RedirectToAction("GameManagement");
        }       
        
        #endregion

        #region Helper Methods
        /// <summary>
        /// Helper method to populate dropdown data for various game views
        /// </summary>
        private void PopulateDropdownData()
        {
            //Send platform selectlist to view for dropdown
            ViewData["platformList"] = new SelectList(db.Platforms, "Id", "Name");

            //Send category selectlist to view for listbox
            ViewData["categoryList"] = new SelectList(db.Catagories, "Id", "Name");

            //Send esrb selectlist to view for dropdown
            ViewData["esrbList"] = new SelectList(esrbList,"Abbreviation", "Rating");
        }
        #endregion

    }
}
