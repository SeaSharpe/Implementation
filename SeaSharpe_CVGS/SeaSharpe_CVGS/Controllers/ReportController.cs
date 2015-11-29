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
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays report of all the details of a specific game
        /// **Can be selected from List Games view**
        /// </summary>
        /// <returns>Game Details view</returns>
        public ActionResult GameDetailsReport()
        {
            return View(db.Games.ToList());
        }

        /// <summary>
        /// Displays report of all games
        /// </summary>
        /// <returns>List Games view</returns>
        public ActionResult ListGamesReport()
        {
             return View(db.Games.ToList());
        }

        /// <summary>
        /// Displays report of all members
        /// </summary>
        /// <returns>List Members view</returns>
        public ActionResult ListMembersReport()
        {
            return View(db.Members.ToList());
        }

        /// <summary>
        /// Displays report of all details of a specified member
        /// **can be selected from List Members view**
        /// </summary>
        /// <returns>Member Details view</returns>
        public ActionResult MemberDetailsReport()
        {
            return View(db.Members.ToList());
        }

        /// <summary>
        /// Displays report of wish list statistics
        /// </summary>
        /// <returns>Wish Lists view</returns>
        public ActionResult WishListsReport()
        {
            return View();
        }

        /// <summary>
        /// Displays report of sales statistics
        /// </summary>
        /// <returns>Sales Report view</returns>
        public ActionResult SalesReport()
        {
            return View();
        }
	}
}