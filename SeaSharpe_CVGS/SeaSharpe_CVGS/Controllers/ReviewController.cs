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
        #region Employee Side
        /// <summary>
        /// Employee Side - List all reviews awaiting review
        /// </summary>
        /// <returns>Review Management view</returns>
        public ActionResult ReviewManagement()
        {
            return View(db.Reviews.ToList());
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
        /// Member Side - List all reviews for a game and show average rating
        /// </summary>
        /// <param name="id">review id</param>
        /// <returns>Reviews/Ratings view for specified game</returns>
        public ActionResult ReviewsRating(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
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
