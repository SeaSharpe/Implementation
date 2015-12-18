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
    /// <summary>
    /// Class for testing the methods of the review controller
    /// </summary>
    class ReviewControllerTest
    {
        TransactionScope _trans;
        ApplicationDbContext db = null;
        ReviewController controller;

        [SetUp]
        public void Init()
        {
            _trans = new TransactionScope();
            // make connection
            db = new ApplicationDbContext();

            //make controller
            controller = new ReviewController { DbContext = db };

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

            Review validTestReview = new Review();
            Game game = db.Games.FirstOrDefault();
            Member member = db.Members.FirstOrDefault();
            validTestReview.Game_Id = game.Id;
            validTestReview.Game = game;
            validTestReview.Author = member;
            validTestReview.Rating = 3;
            validTestReview.Body = "Great Game!";
            validTestReview.Subject = "Review Title";
            validTestReview.IsApproved = false;
            validTestReview.Aprover_Id = null;

            db.Reviews.Add(validTestReview);
            db.SaveChanges();

        }

        [TearDown]
        public void Cleanup()
        {
            _trans.Dispose();
            //db.Database.Delete();
        }

        [Test]
        //Test review index for employee
        public void EmployeeIndex()
        {
            //Get employee from db
            Employee employee = db.Employees.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee, "Employee");

            //Call the controller method
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Index();

            //Check if action being redirected to is ReviewManagement
            Assert.AreEqual("ReviewManagement", result.RouteValues["action"]);
        }

        [Test]
        //Test review index for member
        public void MemberIndex()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Call the controller method
            var result = controller.Index();
            HttpNotFoundResult badRequest = new HttpNotFoundResult();

            //Check if not found result occurs
            Assert.IsInstanceOf(badRequest.GetType(), result);
        }

        [Test]
        //ReviewsRating test for non existent game
        public void ReviewsRatingInvalid()
        {
            //Initialize null game id
            int id = -1;


            //Call the controller method
            var result = controller.ReviewsRating(id);

            //Bad request for comparison
            HttpNotFoundResult badRequest = new HttpNotFoundResult();

            //Check if not found result occurs
            Assert.IsInstanceOf(badRequest.GetType(), result);

        }

        [Test]
        //ReviewsRating test for existent game
        public void ReviewsRatingValid()
        {
            Game game = db.Games.Where(g => g.IsActive).First();

            List<Review> gameReviews = db.Reviews.Where(r => r.Game_Id == game.Id && r.IsApproved).ToList();

            ActionResult result = controller.ReviewsRating(game.Id);

            //Get model from controller action
            var model = ((ViewResult)result).Model as List<Review>;

            CollectionAssert.AreEqual(gameReviews, model);
        }


        [Test]
        //Review details test for existent game
        public void ReviewDetailsValid()
        {
            //Initialize review id
            Review review = db.Reviews.FirstOrDefault();

            //Call the controller method
            var result = controller.PartialReviewDetails(review.Id);

            //Get model from controller action
            var model = ((PartialViewResult)result).Model as Review;

            //Check if not found result occurs
            Assert.AreEqual(review, model);
        }

        [Test]
        //ReviewManagement test
        public void ReviewManagement()
        {
            //Get employee from db
            Employee employee = db.Employees.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee, "Employee");

            //Get list of applicable reviews
            List<Review> reviews = db.Reviews.Where(r => r.Aprover_Id == null && r.Subject != null).ToList();

            //Call controller method
            var result = controller.ReviewManagement();

            //Get model from controller for comparison
            var model = ((ViewResult)result).Model as List<Review>;

            //Compare model to expected reviews
            CollectionAssert.AreEqual(reviews, model);

        }

        [Test]
        //Test the partial review view on the reviewManagement page
        public void selectedReviewPartialView()
        {
            //Get employee from db
            Employee employee = db.Employees.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee, "Employee");

            //Get sample review
            Review review = db.Reviews.FirstOrDefault();

            var result = controller.PartialSelectedReview(review.Id);

            var model = ((PartialViewResult)result).Model as Review;

            Assert.AreEqual(review, model);

        }

        [Test]
        //Test the partial review post back on the reviewManagement page
        public void selectedReviewPartialPost()
        {
            //Get employee from db
            Employee employee = db.Employees.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, employee, "Employee");

            //Get sample review
            Review review = db.Reviews.FirstOrDefault();
            bool originalState = review.IsApproved;

            var result = controller.PartialSelectedReview(review.Id, !originalState);

            Review savedReview = db.Reviews.Find(review.Id);

            Assert.AreEqual(!originalState,savedReview.IsApproved);

        }

        [Test]
        //Test creation of a valid review
        public void partialCreateReviewViewPostValid()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Initialize valid review
            Review validTestReview = new Review();
            Game game = db.Games.FirstOrDefault();
            validTestReview.Game_Id = game.Id;
            validTestReview.Game = game;
            validTestReview.Author = member;
            validTestReview.Rating = 3;
            validTestReview.Body = "test review 123";
            validTestReview.Subject = "Review Title";
            validTestReview.IsApproved = false;
            validTestReview.Aprover_Id = null;

            var result = controller.PartialCreateReview(validTestReview);

            Review savedReview = db.Reviews.Where(r => r.Body == validTestReview.Body).FirstOrDefault();

            Assert.AreEqual(validTestReview.Body, savedReview.Body);
        }

        [Test]
        //Test creation of invalid review
        public void partialCreateReviewViewPostInvalid()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Intialize invalid review
            Review invalidTestReview = new Review();
            Game game = db.Games.FirstOrDefault();
            invalidTestReview.Rating = 3;
            invalidTestReview.Body = "test review 123";
            invalidTestReview.IsApproved = false;
            invalidTestReview.Aprover_Id = null;

            var result = controller.PartialCreateReview(invalidTestReview);

            Review savedReview = db.Reviews.Where(r => r.Body == invalidTestReview.Body).FirstOrDefault();

            Assert.AreEqual(null, savedReview);
        }

        [Test]
        public void partialCreateReviewViewPostRatingOnly()
        {
            //Get member from db
            Member member = db.Members.FirstOrDefault();

            //Set controller context
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member, "Member");

            //Initialize a rating
            Review validTestRating = new Review();
            Game game = db.Games.FirstOrDefault();
            validTestRating.Game_Id = game.Id;
            validTestRating.Game = game;
            validTestRating.Author = member;
            validTestRating.Rating = 3;
            validTestRating.IsApproved = false;
            validTestRating.Aprover_Id = null;

            int originalCount = db.Reviews.Count();

            //Call controller method to create new rating
            controller.PartialCreateReview(validTestRating);

            int newCount = db.Reviews.Count();

            //Compare old and new count to ensure rating was successfully added
            Assert.AreEqual(originalCount + 1, newCount);
        }

    }

}
