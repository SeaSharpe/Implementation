using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeaSharpe_CVGS.Models;

namespace SeaSharpe_CVGS.Controllers
{
    public class GameController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Employee Side
        /// <summary>
        /// Employee Side - List all games
        /// </summary>
        /// <returns>list of games view</returns>
        public ActionResult GameManagement()
        {
            return View(db.Games.ToList());
        }

        /// <summary>
        /// Employee Side - Add a game
        /// </summary>
        /// <returns>return add/edit game view</returns>
        public ActionResult Create()
        {
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
        #endregion
        #region Member side
        /// <summary>
        /// Member Side - List games
        /// </summary>
        /// <returns>list of games view</returns>
        public ActionResult SearchGames()
        {
            return View(db.Games.ToList());
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
