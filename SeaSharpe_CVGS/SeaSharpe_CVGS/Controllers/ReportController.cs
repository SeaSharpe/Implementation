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
            // Get the number of wishlists that exist
            int numberOfWishlists = 0;
            numberOfWishlists = db.WishLists.Count();
            ViewBag.numberOfWishlists = numberOfWishlists;

            // Get the number of members with wishlists
            int membersWithWishlists = 0;
            membersWithWishlists = db.WishLists.Select(w => w.MemberId).Distinct().Count();
            ViewBag.membersWithWishlists = membersWithWishlists;

            // Average number of items in a members wish list
            int itemsInAWishlist = db.WishLists.Count();
            int numberOfPlayersWithWL = db.WishLists.Select(wl => wl.MemberId).Distinct().Count();
            double avgNumOfItemsPerMember = ((double)itemsInAWishlist / (double)numberOfPlayersWithWL) * 100;
            ViewBag.avgNumOfItemsPerMember = itemsInAWishlist;

            return View();
        }

        /// <summary>
        /// Displays report of sales statistics
        /// </summary>
        /// <returns>Sales Report view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult SalesReport()
        {
            // Get the number of sales orders made
            int numberOfSales = 0;
            numberOfSales = db.Orders.Count();
            ViewBag.numberOfSales = numberOfSales;

            // Get the total sales so far
            decimal totalSales = db.Orders.Sum(o => o.OrderItems.Sum(oi => oi.SalePrice));
            ViewBag.totalSales = totalSales.ToString("C");

            // Get the total members who made purchase
            int membersWhoPurchasedItems = 0;
            membersWhoPurchasedItems = db.Orders.Select(o => o.Member.Id).Distinct().Count();
            ViewBag.membersWhoPurchasedItems = membersWhoPurchasedItems;

            // Get total number of members
            int numberOfMembers = 0;
            decimal percentageOfMembersWhoPurchased = 0;

            numberOfMembers = db.Members.Count();
            percentageOfMembersWhoPurchased = ((decimal)membersWhoPurchasedItems / (decimal)numberOfMembers);
            ViewBag.percentageOfMembersWhoPurchased = Math.Round(Convert.ToDecimal(percentageOfMembersWhoPurchased * 100), 2); 
            ViewBag.numberOfMembers = numberOfMembers; // this is only for debugging

            // % of sales from Action Games
            
            return View();
        }

	}
}