using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeaSharpe_CVGS.Models;

namespace SeaSharpe_CVGS.Controllers
{
    public class ReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        #region Multiple Roles
        /// <summary>
        /// checks authorization and redirects to appropriate page
        /// </summary>
        /// <returns>redirect to Review Management or ReviewsRating methods</returns>
        public ActionResult Index()
        {
            //if (Roles.IsUserInRole(@"employee"))
            //{
            //   return View(db.Reviews.ToList());
            return RedirectToAction("ReviewManagement");
            //}
            //else if (Roles.IsUserInRole(@"member"))
            //{
            //    //return SearchGames view
            //      return RedirectToAction("ReviewsRating");
            //}
            //else
            //{
            //    //return SearchGames view
            //      return RedirectToAction("ReviewsRating");
            //}
            
        }        
        #endregion

        #region Employee Side       
        /// <summary>
        /// List all reviews awaiting review
        /// </summary>
        /// <returns>ReviewManagement view</returns>
        public ActionResult ReviewManagement()
        {
            return View(db.Reviews.ToList());
        }

        /// <summary>
        /// displays the currently selected review for approval/rejection
        /// </summary>
        /// <returns>PartialSelectedReview view</returns>
        public PartialViewResult PartialSelectedReview()
        {
            return PartialView();
        }
        /// <summary>
        /// post back for updating review to Accepted
        /// **** No view required ****
        /// </summary>
        /// <param name="review">Review object</param>
        /// <returns>Review Management view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Rating,Subject,Body")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ReviewManagement");
            }
            return View(review);
        }

        /// <summary>
        /// post back - review rejected by employee, delete review
        /// **** no view required ****
        /// </summary>
        /// <param name="id">review id</param>
        /// <returns>Review Management view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("ReviewManagement");
        }

        #endregion

        #region Member Side
        /// <summary>
        /// List all reviews for a game and show average rating
        /// </summary>
        /// <returns>ReviewsRating view</returns>
        public ActionResult ReviewsRating()
        {
            return View(db.Reviews.ToList());
        }

        /// <summary>
        /// Review/Rate a specific game form
        /// **displayed on game details view***
        /// </summary>
        /// <returns>PartialCreateReview view</returns>
        public ActionResult PartialCreateReview()
        {
            return View();
        }

        /// <summary>
        /// Postback for partialCreateReview, creates a new review or updates existing one if it is just a rating
        /// </summary>
        /// <returns>GameDetails view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialCreateReview([Bind(Include = "Id,Rating,Subject,Body,Game_Id")] Review review)
        {
            try
            {
                //Update objects for model
                Game reviewGame = db.Games.FirstOrDefault(g => g.Id == review.Game_Id);
                review.Game = reviewGame;
                review.Author = new Member();

                //Update the model to include binded changes
                ModelState.Clear();
                TryValidateModel(review);

                //Check if model is valid
                if(ModelState.IsValid)
                {
                    Review originalReview = db.Reviews.FirstOrDefault(r => r.Id == review.Id);
                    
                    //Add new review if one does not already exist
                    if (originalReview == null)
                    {
                        db.Reviews.Add(review);
                    }

                    //Update if review already exists
                    else
                    {
                        db.Entry(review).State = EntityState.Modified;                        
                    }
                    db.SaveChanges();
                    TempData["message"] = "Review added.";
                }

            }                

            //Return message to member if exception
            catch (Exception e)
            {
                TempData["message"] = "Error creating review: " + e.GetBaseException().Message;
            }

            TempData["review"] = review;
            return RedirectToAction("Details", "Game", new { id = review.Game_Id });
        }


       /// <summary>
        /// post back for review creation
        /// **** review must be validated by employee before appears in Reviews/Rating list ****
        /// </summary>
       /// <param name="review">Review object</param>
       /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Rating,Subject,Body")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Game/Details");
            }

            return View(review);
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
