using SeaSharpe_CVGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeaSharpe_CVGS.Controllers
{
    public class ReportController : Controller
    {

        /// <summary>
        /// Employee Side - display list of reports that can be created
        /// </summary>
        /// <returns>Index View (Report Management)</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays report of all the details of a specific game
        /// **Can be selected from List Games view**
        /// </summary>
        /// <returns>Game Details view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult GameDetailsReport(int id)
        {
            Game game = db.Games.Find(id);
            return View(game);
        }

        /// <summary>
        /// Displays report of all games
        /// </summary>
        /// <returns>List Games view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult ListGamesReport()
        {
            return View(db.Games.ToList());
        }

        /// <summary>
        /// Displays report of all members
        /// </summary>
        /// <returns>List Members view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult ListMembersReport()
        {
            return View(db.Members.ToList());
        }

        /// <summary>
        /// Displays report of all details of a specified member
        /// **can be selected from List Members view**
        /// </summary>
        /// <returns>Member Details view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult MemberDetailsReport(int id)
        {
            Member member = db.Members.Find(id);
            return View(member);
        }

        /// <summary>
        /// Displays report of wish list statistics
        /// </summary>
        /// <returns>Wish Lists view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult WishListsReport()
        {
            return View();
        }

        /// <summary>
        /// Displays report of sales statistics
        /// </summary>
        /// <returns>Sales Report view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult SalesReport()
        {
            // Get the number of sales
            int numberOfSales = 0;
            numberOfSales = db.Orders.Count();
            ViewBag.numberOfSales = numberOfSales;
            
            return View();
        }

	}
}