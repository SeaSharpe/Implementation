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
        /// search functionality for finding new friends
        /// ** includes partial views for Friends and Family lists**
        /// </summary>
        /// <returns>Search/Show Friends view</returns>
        public ActionResult Index()
        {
            var friendships = db.Friendships.Include(f => f.Friendee).Include(f => f.Friender);

            var list = DummyFriends();

            return View(list);
        }

        /// <summary>
        /// Creates Dummy list of friends
        /// </summary>
        /// <returns></returns>
        List<Friendship> DummyFriends()
        {
            var friendSh = new Friendship();
            List<Friendship> res = new List<Friendship>();

            for (int i = 0; i < 10; i++)
            {
                friendSh.Friendee = DummyMember(i);
                res.Add(friendSh);
            }

            return res;
        }

        /// <summary>
        /// Creates dummy Member 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Member DummyMember(int id)
        {
            Member res = new Member();
            res.Id = 1;
            res.User = new ApplicationUser();

            return res;
        }

        /// <summary>
        /// lists all the member's friends
        /// </summary>
        /// <returns>PartialFriendsList view</returns>
        public ActionResult PartialFriendsList()
        {
            return View();
        }

        /// <summary>
        /// lists all the member's family
        /// </summary>
        /// <returns>PartialFamilyList view</returns>
        public ActionResult PartialFamilyList()
        {
            return View();
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
