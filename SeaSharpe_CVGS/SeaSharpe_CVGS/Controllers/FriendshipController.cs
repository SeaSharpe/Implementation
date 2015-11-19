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
    public class FriendshipController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Members
        /// <summary>
        /// list all friends and search functionality for finding new friends
        /// </summary>
        /// <returns>Search/Show Friends view</returns>
        public ActionResult Index()
        {
            var friendships = db.Friendships.Include(f => f.Friendee).Include(f => f.Friender);
            return View(friendships.ToList());
        }

        /// <summary>
        /// display wishlist of selected friend
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>wishlist view</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friendship friendship = db.Friendships.Find(id);
            if (friendship == null)
            {
                return HttpNotFound();
            }
            return View(friendship);
        }

        /// <summary>
        /// post back for creating friendship (add to friends or add to family)
        /// </summary>
        /// <param name="friendship">friend object</param>
        /// <returns>Search/Show Friends view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="FriendeeId,FrienderId,IsFamilyMember")] Friendship friendship)
        {
            if (ModelState.IsValid)
            {
                db.Friendships.Add(friendship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FriendeeId = new SelectList(db.Members, "Id", "Id", friendship.FriendeeId);
            ViewBag.FrienderId = new SelectList(db.Members, "Id", "Id", friendship.FrienderId);
            return View(friendship);
        }

       /// <summary>
       /// post back for delete friendship
       /// </summary>
       /// <param name="id">friendship id</param>
       /// <returns>Search/Show Friends view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Friendship friendship = db.Friendships.Find(id);
            db.Friendships.Remove(friendship);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
        #endregion
    }
}
