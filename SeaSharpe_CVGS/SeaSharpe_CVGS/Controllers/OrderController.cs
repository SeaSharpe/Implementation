using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeaSharpe_CVGS.Models;
using System.Configuration;
using Stripe;



namespace SeaSharpe_CVGS.Controllers
{
    ///<summary>
    ///This Controller tracks orders for site members and employees.
    ///
    ///**************For Members*********************
    ///Cart:
    ///can add items to a cart
    ///view their cart
    ///delete from their cart
    ///purchase games from their cart
    ///
    ///OrderHistory
    ///View ordered games and the ship dates
    ///
    ///**************For Employees*******************
    ///Order Management:
    ///View all completed orders
    ///with partial views for orders waiting to be processed and a partial view to view the selected order
    ///which also provides options to mark the selected order as "processed" as well as print a shipping 
    ///label with the pertinent details.
    ///</summary>
    public class OrderController : Controller
    {
        #region Multiple Roles
        /// <summary>
        /// checks authorization and redirects to appropriate page
        /// </summary>
        /// <returns>redirect to OrderManagement or OrderHistory methods</returns>
        public ActionResult Index()
        {
            //validate user role
            string userId = CurrentUser.Id;
            var member = db.Members.FirstOrDefault(m => m.User.Id == userId);
            var employee = db.Employees.FirstOrDefault(m => m.User.Id == userId);
            if (member != null)
            {
                return RedirectToAction("OrderHistory");
            }
            else if (employee != null)
            {
                return RedirectToAction("OrderManagement");
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Employee Side
        /// <summary>
        /// list all completed orders
        /// </summary>
        /// <returns>OrderManagement view</returns>
        public ActionResult OrderManagement(int id = 0)
        {
            /*
             * If employee
             * show all orders with IsProcessed == true
             */
            string userId = CurrentUser.Id;
            var employee = db.Employees.FirstOrDefault(m => m.User.Id == userId);
            if (employee == null)
            {
                return RedirectToAction("OrderHistory");
            }

            IEnumerable<Order> completedOrders = db.Orders
                .Where(o => o.IsProcessed == true)
                .Include(m => m.Member).Include(u => u.Aprover.User).Include(oi => oi.OrderItems).OrderByDescending(d => d.ShipDate);

            // Nicole added - only display partial selected order when order is selected
            ViewData["order"] = db.Orders.Find(id);
            return View(completedOrders);
        }

        /// <summary>
        /// Gets order and uses it to create a shipping label
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Shipping View</returns>
        public ActionResult Shipping(int id)
        {
            string userId = CurrentUser.Id;
            var employee = db.Employees.FirstOrDefault(m => m.User.Id == userId);
            if (employee == null)
            {
                return RedirectToAction("OrderHistory");
            }
            Order orderToShip = db.Orders.Find(id);
            return View(orderToShip);
        }

        /// <summary>
        /// list all orders waiting to be processed
        /// </summary>
        /// <returns>PartialOutstandingOrders partial view</returns>
        public ActionResult PartialOutstandingOrders()
        {
            /*
             * If employee
             * show all orders with IsProcessed == false
             */
            string userId = CurrentUser.Id;
            var employee = db.Employees.FirstOrDefault(m => m.User.Id == userId);
            if (employee == null)
            {
                return RedirectToAction("OrderHistory");
            }

            IEnumerable<Order> outstandingOrders = db.Orders
                .Where(o => o.OrderPlacementDate != null && o.IsProcessed == false)
                .Include(m => m.Member.User).Include(oi => oi.OrderItems).OrderBy(d => d.OrderPlacementDate);

            return View(outstandingOrders);
        }

        /// <summary>
        /// list order items in selected order and provides controls to mark
        /// the order as "processed" as well as print a shipping label
        /// </summary>
        /// <returns>PartialSelectedOrder partial view</returns>
        public ActionResult PartialSelectedOrder(int id = 0)
        {
            /*
             * if employee
             * all games where orderId == id 
             * (add param)
             */
            string userId = CurrentUser.Id;
            var employee = db.Employees.FirstOrDefault(m => m.User.Id == userId);
            if (employee == null)
            {
                return RedirectToAction("OrderHistory");
            }

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

        /// <summary>
        /// Finds current employee and adds their name as the order's approver, as well as setting the ship date
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>OrderManagement View</returns>
        public ActionResult MarkAsProcessed(int id = 0)
        {
            string userId = CurrentUser.Id;
            var employee = db.Employees.FirstOrDefault(m => m.User.Id == userId);
            if (employee == null)
            {
                return RedirectToAction("OrderHistory");
            }

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
            //Employee employee = db.Employees.FirstOrDefault(m => m.User.Id == CurrentUser.Id);

            //TODO remove (placeholder for employee)

            order.IsProcessed = true;
            order.Aprover = employee;
            order.ShipDate = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("OrderManagement");
        }

        #endregion
        #region Member Side
        /// <summary>
        /// list all orders waiting to be processed
        /// </summary>
        /// <returns>OrderHistory view</returns>
        public ActionResult OrderHistory()
        {
            //validate user role
            string userId = CurrentUser.Id;
            var member = db.Members.FirstOrDefault(m => m.User.Id == userId);
            if (member == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //get orders for member (not cart)
            var exists = db.Orders.Where(m => m.Member.Id == member.Id).Where(d => d.OrderPlacementDate != null).Any();

            if (!exists)
            {
                //empty cart
                TempData["message"] = "No order history";
                return View(Enumerable.Empty<OrderHistoryViewModel>().ToList());
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
            int gameId;

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
                    gameId = db.Games.Where(g => g.Id == orderItem.GameId).Select(ga => ga.Id).First();
                    ohvm = new OrderHistoryViewModel(orderDate, shipDate, gameName, platformName, pricePaid,gameId);
                    userOrderHistory.Add(ohvm);
                }
            }

            return View(userOrderHistory);
        }

        /// <summary>
        /// Show order items in cart order
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>Cart view</returns>
        public ActionResult Cart(int? id)
        {
            //validate user role
            string userId = CurrentUser.Id;
            var member = db.Members.FirstOrDefault(m => m.User.Id == userId);
            if (member == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //validate that the member has a cart
            var exists = db.Orders.Where(m => m.Member.Id == member.Id).Where(d => d.OrderPlacementDate == null).Any();

            if (!exists)
            {
                //empty cart
                TempData["message"] = "Cart is empty";
                return View(Enumerable.Empty<CartViewModel>().ToList());
            }

            //This gets the cart order id
            int orderId = db.Orders.Where(m => m.Member.Id == member.Id).Where(d => d.OrderPlacementDate == null).First().Id;

            //get all gameIds for order Id
            IEnumerable<OrderItem> orderItems = db.OrderItems.Where(o => o.OrderId == orderId);

            List<CartViewModel> cartItems = new List<CartViewModel>();

            foreach (var orderItem in orderItems)
            {
                CartViewModel cvm = new CartViewModel(orderItem, false);
                cartItems.Add(cvm);
            }

            return View(cartItems);
        }

        /// <summary>
        /// Cart purchase postback action
        /// </summary>
        /// <param name="stripeToken">
        ///     The token provided to the client to track this purchase
        /// </param>
        /// <param name="stripeTokenType">The type of token, usualy "card"</param>
        /// <param name="stripeEmail">
        ///     The email the customer provided in the stripe payment dialogue box
        /// </param>
        /// <returns>
        ///     Returns a view displaying the status of the payment to the user.
        /// </returns>
        [HttpPost]
        public ActionResult Cart(string stripeToken, string stripeTokenType, string stripeEmail)
        {
            var memberOrder = db.Orders.
                OrderBy(order => order.Id).
                FirstOrDefault(order => order.Member.Id == CurrentMember.Id &&
                                        order.OrderPlacementDate == null);

            //Price in cents
            int price = Decimal.ToInt32(100 * memberOrder.OrderItems.Sum(orderItem => orderItem.SalePrice));

            var chargeOptions = new StripeChargeCreateOptions
            {
                Amount = price,
                Currency = "cad",
                ReceiptEmail = CurrentUser.Email,
                Metadata = new Dictionary<string, string>() { { "memberId", memberOrder.Member.Id.ToString() }, 
                                                              { "orderId", memberOrder.Id.ToString() }, 
                                                              { "userId", CurrentUser.Id } },
                Source = new StripeSourceOptions
                {
                    TokenId = stripeToken
                }
            };

            var chargeService = new StripeChargeService();

            try
            {
                TempData["message"] = "Charged card";
                var stripeCharge = chargeService.Create(chargeOptions);
                ViewBag.StripeCharge = stripeCharge;
                memberOrder.OrderPlacementDate = DateTime.Now;

                // Billing address will be the user's first address
                memberOrder.BillingAddress = db.Addresses.
                    OrderBy(addr => addr.Id).
                    FirstOrDefault(addr => addr.Member.Id == CurrentMember.Id);

                // Shipping address will be the user's last address
                memberOrder.ShippingAddress = db.Addresses.
                    OrderByDescending(addr => addr.Id).
                    FirstOrDefault(addr => addr.Member.Id == CurrentMember.Id);

                db.SaveChanges();
                return Cart(null);
            }
            catch (Exception se)
            {
                TempData["message"] = se.Message;
                return Cart(null);
            }
        }

        /// <summary>
        /// Member side - Add a specific game to cart
        /// </summary>
        /// <param name="id">game id</param>
        /// <returns>game details view</returns>
        public ActionResult AddToCart(int? id)
        {
            //validate user role
            string userId = CurrentUser.Id;
            var member = db.Members.FirstOrDefault(m => m.User.Id == userId);
            if (member == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //check whether the member has a cart
            var theCart = db.Orders.FirstOrDefault(m => m.Member.Id == member.Id && m.OrderPlacementDate == null);

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

            return RedirectToAction("Cart");
        }

        /// <summary>
        /// Accepts post from cart view, determines whether the member clicked "Remove Selected"
        /// or "Checkout Now" and then directs to the appropriate method
        /// </summary>
        /// <param name="cart">CartViewModel object</param>
        /// <param name="submit">String value of the button clicked</param>
        /// <returns>Cart View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlterCart(CartViewModel[] cart, string submit)
        {
            //validate user role
            string userId = CurrentUser.Id;
            var member = db.Members.FirstOrDefault(m => m.User.Id == userId);
            if (member == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (cart == null)
            {
                return RedirectToAction("Cart");
            }

            if (submit == "Remove Selected")
            {
                Delete(cart);
            }
            
            return RedirectToAction("Cart");
        }

        /// <summary>
        /// Checks the checkboxes (represented in the CartViewModel object as download and hardcopy booleans,
        /// deletes the items, and then checks to see if the cart has any items left, if it does not it deletes
        /// cart (which is an order with no order items at that point)
        /// </summary>
        /// <param name="cart">CartViewModel object</param>
        public void Delete(CartViewModel[] cart)
        {
            //get original order count
            Order originalOrder = db.Orders.Find(cart.First().item.OrderId);

            //check number of items removes
            int itemsRemoved = 0;
            int originalNumberOfItems = originalOrder.OrderItems.Count();

            foreach (var cvm in cart)
            {
                //remove selected items
                if (cvm.remove)
                {
                    itemsRemoved++;
                    OrderItem orderItem = db.OrderItems.Where(g => g.GameId == cvm.item.GameId && g.OrderId == cvm.item.OrderId).First();
                    db.OrderItems.Remove(orderItem);
                    db.SaveChanges();
                }
            }

            db.SaveChanges();

            //if order is now empty
            if (originalNumberOfItems == itemsRemoved)
            {
                //delete the now empty order
                db.Orders.Remove(originalOrder);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the entire cart by getting the member id and checking for a valid cart and then deleting it
        /// </summary>
        /// <returns>Cart View</returns>
        public ActionResult DeleteCart()
        {
            //validate user role
            string userId = CurrentUser.Id;
            var member = db.Members.FirstOrDefault(m => m.User.Id == userId);
            if (member == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //validate that memberId is valid
            var exists = db.Orders.Where(m => m.Member.Id == member.Id).Where(d => d.OrderPlacementDate == null).Any();

            if (!exists)
            {
                //empty cart
                TempData["message"] = "Cart is empty";
                return RedirectToAction("Cart");
            }

            //This gets the cart order
            Order cart = db.Orders.Where(m => m.Member.Id == member.Id).Where(d => d.OrderPlacementDate == null).First();

            //delete cart
            db.Orders.Remove(cart);
            db.SaveChanges();

            return RedirectToAction("Cart");
        }

        #endregion
    }
}
