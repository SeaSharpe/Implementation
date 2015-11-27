using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeaSharpe_CVGS.Models;
using SeaSharpe_CVGS.Migrations;
using Microsoft.AspNet.Identity;

namespace SeaSharpe_CVGS.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Multiple Roles
        /// <summary>
        /// checks authorization and redirects to appropriate page
        /// </summary>
        /// <returns>redirect to OrderManagement or OrderHistory methods</returns>
        public ActionResult Index()
        {
            //if (Roles.IsUserInRole(@"employee"))
            //{
            //    return View(db.Orders.ToList());
            return RedirectToAction("OrderManagement");
            //}
            //else if (Roles.IsUserInRole(@"member"))
            //{
            //    //return ViewEvents view
            //      return RedirectToAction("OrderHistory");
            //}
            //else
            //{
            //    //return ViewEvents view
            //      return RedirectToAction("OrderHistory");
            //}
        }
        
        #endregion

        #region Employee Side
        /// <summary>
        /// list all completed orders
        /// </summary>
        /// <returns>Order Management view</returns>
        public ActionResult OrderManagement()
        {
            /*
             * If employee
             * all orders with employeeId != null
             */ 
            return View(db.Orders.ToList());
        }

        /// <summary>
        /// list all orders waiting to be processed
        /// </summary>
        /// <returns>Outstanding orders partial view</returns>
        public ActionResult PartialOutstandingOrders()
        {
            /*
             * If employee
             * all orders with employeeId == null
             */ 
            return View(db.Orders.ToList());
        }

        /// <summary>
        /// list order items in selected order
        /// </summary>
        /// <returns>selected order partial view</returns>
        public ActionResult PartialSelectedOrder()
        {
            /*
             * if employee
             * all games where orderId == id 
             * (add param)
             */ 
            return View();
        }
        /// <summary>
        /// post back order updated to processed
        /// *** No view required ***
        /// </summary>
        /// <param name="order">order object</param>
        /// <returns>Order Management view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "Id,OrderPlacementDate,ShipDate,IsProcessed")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OrderManagement");
            }
            return View(order);
        }
        #endregion
        #region Member Side
        /// <summary>
        /// list all orders waiting to be processed
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderHistory()
        {
            /*
             * if member
             * all orders orderplacementdate != null and employeeId != null
             */ 
            return View(db.Orders.ToList());
        }

        /// <summary>
        /// Show order items in cart order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Cart view</returns>
        public ActionResult Cart()
        {
            /*
             * TODO:
             * get member id
             * Clean up view
             * Create checkboxes and buttons
             * Verfiy that only one cart can exist at a time
             */

            //get userid
            //int memberId = db.Members.FirstOrDefault(m => m.User.Id == User.Identity.GetUserId()).Id;

            //placeholder for getting member id
            int memberId = 40;
            

            //validate that memberId is valid
            var exists = db.Orders.Where(m => m.Member.Id == memberId).Where(d => d.OrderPlacementDate == null).Any();

            if (!exists)
            {
                //empty cart
                TempData["EmptyCart"] = "Cart is empty";
                return View(Enumerable.Empty<Game>());
            }

            //This gets the cart order id
            int orderId = db.Orders.Where(m => m.Member.Id == memberId).Where(d => d.OrderPlacementDate == null).First().Id;

            //get all gameIds for order Id
            var orderItemIds = db.OrderItems.Where(o => o.OrderId == orderId).Select(i => i.GameId);

            //get all games for gameIds
            IEnumerable<Game> games = db.Games.Where(g => orderItemIds.Contains(g.Id)).Include(c => c.Platform);

            return View(games);
            /*
            var config = new Configuration();
            
            config.SeedDebug(db);
            return View();
             * */
        }

        /// <summary>
        /// Member Side - Order Created when first item is added to cart
        /// **** No view required ****
        /// </summary>
        /// <returns>Cart view</returns>
        public ActionResult Create()
        {
            return RedirectToAction("Cart");
        }

        /// <summary>
        /// Post back for order creation
        /// </summary>
        /// <param name="order">order object</param>
        /// <returns>Cart view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderPlacementDate,ShipDate,IsProcessed")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Cart");
            }

            return View(order);
        }

        /// <summary>
        /// Member side - Add a specific game to cart
        /// ****No view required****
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>game details view</returns>
        public ActionResult AddToCart(int? id)
        {
            /*
             * add item
             */

            //get userid
            //int memberId = db.Members.FirstOrDefault(m => m.User.Id == User.Identity.GetUserId()).Id;

            //placeholder for getting member id
            int memberId = 40;

            //check whether the member has a cart
            var exists = db.Orders.Where(m => m.Member.Id == memberId).Where(d => d.OrderPlacementDate == null).Any();

            if (!exists)
            {
                //create cart
                //add item to cart
                //return to details
                return RedirectToAction("details", "Game", new { id });
            }

            //This gets the cart order id
            int orderId = db.Orders.Where(m => m.Member.Id == memberId).Where(d => d.OrderPlacementDate == null).First().Id;

            //add item to order
            Order order = db.Orders.Find(orderId);

            //add item to order
            

            return RedirectToAction("details", "Game", new { id });
        }

        /// <summary>
        /// post back order updated when items added to cart
        /// *** No view required ***
        /// </summary>
        /// <param name="order">order object</param>
        /// <returns>Cart view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCartWithItem([Bind(Include = "Id,OrderPlacementDate,ShipDate,IsProcessed")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Cart");
            }
            return View(order);
        }

        /// <summary>
        /// post back for deletion of cart item
        /// </summary>
        /// <param name="id">orderItem id</param>
        /// <returns>Cart view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int itemId)
        {
            Order order = db.Orders.Find(itemId);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }

        /// <summary>
        /// post back for deletion of entire cart
        /// </summary>
        /// <param name="id">order id</param>
        /// <returns>Cart view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCartConfirmed(int orderId)
        {
            Order order = db.Orders.Find(orderId);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }
        
        #endregion
       

        /// <summary>
        /// garbage collection
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
