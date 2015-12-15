using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SeaSharpe_CVGS.Controllers;
using SeaSharpe_CVGS.Models;
using System.Web.Mvc;
using System.Web;
using System.Security.Principal;
using Moq;
using System.Globalization;
using System.Net;
using System.IO;
using System.Transactions;

namespace SeaSharpe_CVGS.Tests.Controllers
{
    
    public class OrderControllerTest
    {
        ApplicationDbContext db = null;

        [SetUp]
        public void Init()
        {
            // make connection
            
            db = new ApplicationDbContext();

            // seed the database if empty
            if (db.Members.Count() == 0)
            {
                new Migrations.Configuration().SeedDebug(db);
                Console.WriteLine("Seeded DB");
            }
            else
            {
                Console.WriteLine("DB Already seeded");
            }
        }

        [TearDown]
        public void Cleanup()
        {
            //db.Database.Delete();
        }

        [Test]
        public void OrderHistoryReturnsView()
        {
            var member = db.Members.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member);
            ViewResult result = controller.OrderHistory() as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void CartReturnsView()
        {
            var member = db.Members.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member);
            ViewResult result = controller.Cart(1) as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void MemberRedirectedFromOrderMgmntToOrderHistory()
        {
            var member = db.Members.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member);
            RedirectToRouteResult result = (RedirectToRouteResult)controller.OrderManagement();

            Assert.AreEqual("OrderHistory", result.RouteValues["action"]);
        }

        [Test]
        public void MemberRedirectedFromShippingToOrderHistory()
        {
            var member = db.Members.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member);
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Shipping(1);

            Assert.AreEqual("OrderHistory", result.RouteValues["action"]);
        }

        [Test]
        public void MemberRedirectedFromPartialOutstandingOrdersToOrderHistory()
        {
            var member = db.Members.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member);
            RedirectToRouteResult result = (RedirectToRouteResult)controller.PartialOutstandingOrders();

            Assert.AreEqual("OrderHistory", result.RouteValues["action"]);
        }

        [Test]
        public void MemberRedirectedFromPartialSelectedOrderToOrderHistory()
        {
            var member = db.Members.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member);
            RedirectToRouteResult result = (RedirectToRouteResult)controller.PartialSelectedOrder();

            Assert.AreEqual("OrderHistory", result.RouteValues["action"]);
        }

        [Test]
        public void MemberRedirectedFromMarkAsProcessedToOrderHistory()
        {
            var member = db.Members.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member);
            RedirectToRouteResult result = (RedirectToRouteResult)controller.MarkAsProcessed();

            Assert.AreEqual("OrderHistory", result.RouteValues["action"]);
        }

        [Test]
        public void MemberRedirectedFromIndexToOrderHistory()
        {
            var member = db.Members.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member);
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Index();

            Assert.AreEqual("OrderHistory", result.RouteValues["action"]);
        }


        //employees
        [Test]
        public void OrderManagementReturnsView()
        {
            var employee = db.Employees.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee);
            ViewResult result = controller.OrderManagement() as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void ShippingReturnsView()
        {
            var employee = db.Employees.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee);
            ViewResult result = controller.Shipping(1) as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void PartialOutstandingOrdersReturnsView()
        {
            var employee = db.Employees.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee);
            ViewResult result = controller.PartialOutstandingOrders() as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void PartialSelectedOrderReturnsView()
        {
            var employee = db.Employees.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee);
            ViewResult result = controller.PartialSelectedOrder() as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void MarkAsProcessedReturnsView()
        {
            var employee = db.Employees.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee);
            RedirectToRouteResult result = (RedirectToRouteResult)controller.MarkAsProcessed();

            Assert.AreEqual("OrderManagement", result.RouteValues["action"]);
        }

        [Test]
        public void EmployeeRedirectedFromIndexToOrderManagement()
        {
            var employee = db.Employees.OrderBy(m => m.UserId).FirstOrDefault();
            var controller = new OrderController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee);
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Index();

            Assert.AreEqual("OrderManagement", result.RouteValues["action"]);
        }
    }
}
