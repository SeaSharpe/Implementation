using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeaSharpe_CVGS.Models;
using System.Web.Security;
using System.Collections.Generic;

namespace SeaSharpe_CVGS.Controllers
{
    public class GameController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Multiple Roles

        /// <summary>
        /// Employee side - list all games
        /// Member side - search feature, list matching games
        /// </summary>
        /// <returns>list of games view</returns>
        public ActionResult Index()
        {
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
        public ActionResult SearchGames()
        {
            List<Game> gameList = db.Games.ToList();
            return View(gameList);
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
            //Send platform selectlist to view for dropdown
            ViewBag.platformList = new SelectList(db.Platforms, "Id", "Name");

            //Send category selectlist to view for dropdown
            ViewBag.categoryList = new SelectList(db.Catagories, "Id", "Name");

            return View();
        }

        /// <summary>
        /// Employee Side - post back for game creation, attempt to save to db
        /// </summary>
        /// <param name="game">game object</param>
        /// <returns>view of games' list</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,ReleaseDate,SuggestedRetailPrice")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("GameManagement");
            }

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
        public ActionResult Edit([Bind(Include="Id,Name,ReleaseDate,SuggestedRetailPrice")] Game game)
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
