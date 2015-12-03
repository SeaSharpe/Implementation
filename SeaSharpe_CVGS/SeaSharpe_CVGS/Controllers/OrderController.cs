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

        //placeholder for getting member id
        private int memberId = 38;

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
        public ActionResult OrderManagement(int id = 0)
        {
            /*
             * If employee
             * all orders with IsProcessed == true
             */

            IEnumerable<Order> completedOrders = db.Orders
                .Where(o => o.IsProcessed == true)
                .Include(m => m.Member).Include(u => u.Aprover.User).Include(oi => oi.OrderItems).OrderBy(d => d.ShipDate);

            return View(completedOrders);
        }

        /// <summary>
        /// list all orders waiting to be processed
        /// </summary>
        /// <returns>Outstanding orders partial view</returns>
        public ActionResult PartialOutstandingOrders()
        {
            /*
             * If employee
             * all orders with IsProcessed == false
             */
            IEnumerable<Order> outstandingOrders = db.Orders
                .Where(o => o.OrderPlacementDate != null && o.IsProcessed == false)
                .Include(m => m.Member.User).Include(oi => oi.OrderItems).OrderBy(d => d.OrderPlacementDate);

            return View(outstandingOrders);
        }

        /// <summary>
        /// list order items in selected order
        /// </summary>
        /// <returns>selected order partial view</returns>
        public ActionResult PartialSelectedOrder(int id = 0)
        {
            /*
             * if employee
             * all games where orderId == id 
             * (add param)
             */

            Order order;
            if (id == 0)
            {
                order = new Order();
                order.Id = -1;
            }
            else
            {
                order = db.Orders.Find(id);
            }

            return View(order);
        }

        public ActionResult MarkAsProcessed(int id = 0)
        {
            //if no item selected
            if (id == 0)
            {
                //Message should only appear in SelectedOrderPartialView, so unique TempData key is given
                TempData["messageDan1"] = "Please select a game to Process";
                return RedirectToAction("OrderManagement");
            }
            Order order = db.Orders.Find(id);

            //if no valid item selected
            if (order == null)
            {
                //Message should only appear in SelectedOrderPartialView, so unique TempData key is given
                TempData["messageDan1"] = "Please select a valid game";
                return RedirectToAction("OrderManagement");
            }

            //if order is already processed
            if (order.IsProcessed == true)
            {
                //Message should only appear in SelectedOrderPartialView, so unique TempData key is given
                TempData["messageDan1"] = "Order has already been processed";
                return RedirectToAction("OrderManagement");
            }

            //get approver
            //Employee employee = db.Employees.FirstOrDefault(m => m.User.Id == User.Identity.GetUserId());

            //get approver placeholder
            Employee approver = db.Employees.Find(1);


            order.IsProcessed = true;
            order.Aprover = approver;
            db.SaveChanges();

            return RedirectToAction("OrderManagement");
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
            var member = db.Members.Find(memberId);

            //get orders for member (not cart)
            var exists = db.Orders.Where(m => m.Member.Id == member.Id).Where(d => d.OrderPlacementDate != null).Any();

            if (!exists)
            {
                //empty cart
                TempData["EmptyCart"] = "No order history";
                return View(Enumerable.Empty<Game>());
            }

            //get all gameIds for order Id
            IEnumerable<int> gameIds = db.OrderItems.
                Where(o => o.Order.Member.Id == member.Id && o.Order.OrderPlacementDate != null)
                .Select(i => i.GameId);

            IEnumerable<Order> orderIds = db.Orders.Where(m => m.Member.Id == member.Id && m.OrderPlacementDate != null).ToList();

            DateTime orderDate;
            DateTime shipDate;
            string gameName;
            string platformName;
            decimal pricePaid;

            List<OrderHistoryViewModel> userOrderHistory = new List<OrderHistoryViewModel>();
            OrderHistoryViewModel ohvm;

            foreach (var item in orderIds)
            {
                orderDate = (DateTime)item.OrderPlacementDate;
                if (item.ShipDate != null)
                {
                    shipDate = (DateTime)item.ShipDate;
                }
                else
                {
                    shipDate = DateTime.Parse("1900-01-01 00:00:00");
                }
                

                IEnumerable<OrderItem> orderItemIds = db.OrderItems.Where(oi => oi.OrderId == item.Id).ToList();
                foreach (var orderItem in orderItemIds)
                {
                    
                    gameName = db.Games.Where(g => g.Id == orderItem.GameId).Select(ga => ga.Name).First();
                    platformName = db.Games.Include(p => p.Platform).Where(g => g.Id == orderItem.GameId).Select(ga => ga.Platform.Name).First().ToString();
                    pricePaid = orderItem.SalePrice;
                    ohvm = new OrderHistoryViewModel(orderDate, shipDate, gameName, platformName, pricePaid);
                    userOrderHistory.Add(ohvm);
                }
            }

            //get all games for gameIds
            //IEnumerable<Game> games = db.Games.Where(g => orderItemIds.Contains(g.Id)).Include(c => c.Platform);
            return View(userOrderHistory);
        }

        /// <summary>
        /// Show order items in cart order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Cart view</returns>
        public ActionResult Cart()
        {
            /* TODO:
             * Clean up view
             * Create checkboxes and buttons*/

            //get userid
            //var member = db.Members.FirstOrDefault(m => m.User.Id == User.Identity.GetUserId());

            //validate that memberId is valid
            var exists = db.Orders.Where(m => m.Member.Id == memberId).Where(d => d.OrderPlacementDate == null).Any();

            if (!exists)
            {
                //empty cart
                TempData["message"] = "Cart is empty";
                return View(Enumerable.Empty<Game>());
            }

            //This gets the cart order id
            int orderId = db.Orders.Where(m => m.Member.Id == memberId).Where(d => d.OrderPlacementDate == null).First().Id;

            //get all gameIds for order Id
            var orderItemIds = db.OrderItems.Where(o => o.OrderId == orderId).Select(i => i.GameId);

            
            //get all games for gameIds
            IEnumerable<Game> games = db.Games.Where(g => orderItemIds.Contains(g.Id)).Include(c => c.Platform);

            return View(games);
        }

        /// <summary>
        /// Member Side - OrderPlacement date is set (changes it from a cart to an order)
        /// **** No view required ****
        /// </summary>
        /// <returns>Cart view</returns>
        public ActionResult Create()
        {
            return RedirectToAction("Cart");
        }

        /// <summary>
        /// Member side - Add a specific game to cart
        /// ****No view required****
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>game details view</returns>
        public ActionResult AddToCart(int? id)
        {
            //get userid
            //var member = db.Members.FirstOrDefault(m => m.User.Id == User.Identity.GetUserId());
            
            //placeholder
            var member = db.Members.Find(memberId);

            //check whether the member has a cart
            var theCart = db.Orders.FirstOrDefault(m => m.Member.Id == memberId && m.OrderPlacementDate == null);

            //check that game is valid
            var game = db.Games.Find(id);
            if (game == null)
            {
                TempData["message"] = "Invalid Game";
                return RedirectToAction("Index", "Game");
            }

            //create new cart if no pre-existing
            if (theCart == null)
            {
                //new order, addresses are default null until checkout
                theCart = new Order { Member = member };

                //add order to db
                db.Orders.Add(theCart);
                db.SaveChanges();
            }

            //check if game already exists
            if (db.OrderItems.Where(m => m.OrderId == theCart.Id && m.GameId == game.Id).FirstOrDefault() != null)
            {
                //currently this stops the addToCart, if we add a quantity column to the orderItems table, it could increment the quantity instead
                TempData["message"] = game.Name + " already exists in cart";
                return RedirectToAction("details", "Game", new { id });
            }

            OrderItem orderItem = new OrderItem { Game = game, GameId = game.Id, OrderId = theCart.Id, Order = theCart, SalePrice = game.SuggestedRetailPrice };
            
            db.OrderItems.Add(orderItem);
            db.SaveChanges();

            TempData["message"] = game.Name + " added to cart";

            return RedirectToAction("details", "Game", new { id });
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
            /*
             * 
             */
            
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
