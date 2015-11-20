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

namespace SeaSharpe_CVGS.Controllers
{
    public class GameController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Dictionary containing ESRB ratings
        public static List<string> esrbList = new List<string>
            {
                {"EC"},{"E"},{"E10"},{"T"},{"M"},{"AO"}
            };
        

        #region Multiple Roles

        /// <summary>
        /// Employee side - list all games
        /// Member side - search feature, list matching games
        /// </summary>
        /// <returns>list of games view</returns>
        public ActionResult Index()
        {
            //Incomplete: temporary code for testing purposes until roles are working
            return RedirectToAction("SearchGames");

            //User is employee, redirect to GameManagement
            if (Roles.IsUserInRole(@"employee"))
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
        /// <returns>List of games view</returns>
        public ActionResult SearchGames(string nameSearch, int[] platformSearch, int[] categorySearch, string[] esrbSearch)
        {           
            IEnumerable<Game> gameList = db.Games;
            //Name search query
            if(nameSearch != null)
            {
                gameList = gameList.Where(g => g.Name.Contains(nameSearch));
            }

            //Platform search query
            if(platformSearch != null)
            {
                gameList = gameList.Where(g => platformSearch.Contains(g.Platform.Id));
            }

            //Category search query
            if (categorySearch != null)
            {
                IEnumerable<Category> selectedCategories = db.Catagories.Where(c => categorySearch.Contains(c.Id));
                gameList = gameList.Where(g => selectedCategories.Intersect(g.Categories).Any());
            }

            //Esrb search query
            if (esrbSearch != null)
            {
                
                gameList = gameList.Where(g => esrbSearch.Contains(g.ESRB));
            }
            populateDropdownData();
            return View(gameList.ToList());
        }

        /// <summary>
        /// Get Single Game
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
            return View(game);
        }

        /// <summary>
        /// Displays game list page for employees
        /// </summary>
        /// <returns>List of games view</returns>
        public ActionResult GameManagement()
        {
            return View();
        }

        #endregion

        #region Employee Side

        /// <summary>
        /// Employee Side - Add a game
        /// </summary>
        /// <returns>return add/edit game view</returns>
       
        public ActionResult Create()
        {
            populateDropdownData();
            return View();
        }

        /// <summary>
        /// Employee Side - post back for game creation, attempt to save to db
        /// </summary>
        /// <param name="game">game object</param>
        /// <returns>view of games' list</returns>
        //ADD EMPLOYY AUTH
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ReleaseDate,SuggestedRetailPrice, ImagePath, Publisher, ESRB")] Game game, int? Platform, int[] Categories)
        {            
            try
            {
                //TODO: ensure that datetime for releasedate is being validated properly

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
                TempData["error"] = "Error creating game: " + e.GetBaseException().Message;                
            }

            Create();
            return View(game);                        
        }

        /// <summary>
        /// Employee side - edit game
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>add/edit game view</returns>
        public ActionResult Edit(int? id)
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
            return View(game);
        }

        /// <summary>
        /// Employee side - post back for edit game, save to db
        /// </summary>
        /// <param name="game">game object</param>
        /// <returns>list of games view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,ReleaseDate,SuggestedRetailPrice")] Game game, int Platform)
        {

            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GameManagement");
            }
            return View(game);
        }

        /// <summary>
        /// Employee -side post back for delete game.  **no delete view, delete button on list of games view***
        /// ****No view required****
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>list of games view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("GameManagement");
        }        
        
        /// <summary>
        /// Member side - Add a specific game to wish list
        /// ****No view required****
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>game details view</returns>
        public ActionResult AddToWishList(int? id)
        {
            return RedirectToAction("Details");
        }
        /// <summary>
        /// Member side - Download a specific game
        /// ****No view required****
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>game details view</returns>
        public ActionResult Download(int? id)
        {
            return RedirectToAction("Details");
        }
        
        #endregion

        #region Helper Methods
        /// <summary>
        /// Helper method to populate dropdown data for various game views
        /// </summary>
        private void populateDropdownData()
        {
            //Send platform selectlist to view for dropdown
            ViewData["platformList"] = new SelectList(db.Platforms, "Id", "Name");

            //Send category selectlist to view for listbox
            ViewData["categoryList"] = new SelectList(db.Catagories, "Id", "Name");

            //Send esrb seletlist to view for dropdown
            ViewData["esrbList"] = new SelectList(esrbList);
        }
        #endregion  

        /// <summary>
        /// garbage disposal
        /// </summary>
        /// <param name="disposing">garbage</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
