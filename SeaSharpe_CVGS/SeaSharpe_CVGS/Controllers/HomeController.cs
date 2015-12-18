using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeaSharpe_CVGS.Models;

namespace SeaSharpe_CVGS.Controllers
{
    /// <summary>
    /// Controller class for our main pages
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Our landing page action
        /// </summary>
        /// <returns>A view containing some of our hotest games</returns>
        public ActionResult Index()
        {
            IQueryable<Game> listOfGames = db.Games.Where(g => g.IsActive).OrderByDescending(x => x.Id).Take(16);
            return View(listOfGames.ToList());
        }

        /// <summary>
        /// Our contact page action
        /// </summary>
        /// <returns>A view with our contact info</returns>
        public ActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// This page is for employees to re-build the database
        /// </summary>
        /// <returns>Returns a count of all tables</returns>
        [Authorize(Roles = "Employee")]
        public JsonResult SeedDatabase()
        {
            var db = new SeaSharpe_CVGS.Models.ApplicationDbContext();
            try
            {
                (new SeaSharpe_CVGS.Migrations.Configuration()).SeedDebug(db);
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Exception = new { Message = e.Message, Stacktrace = e.StackTrace } }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Success = true,
                Users = db.Users.Count(),
                Employees = db.Employees.Count(),
                Members = db.Members.Count(),
                Events = db.Events.Count(),
                Addresses = db.Addresses.Count(),
                Orders = db.Orders.Count(),
                Games = db.Games.Count(),
                Platforms = db.Platforms.Count(),
                Categories = db.Catagories.Count(),
                Reviews = db.Reviews.Count(),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}