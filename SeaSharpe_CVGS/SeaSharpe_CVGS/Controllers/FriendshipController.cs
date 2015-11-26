using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SeaSharpe_CVGS.Models;

namespace SeaSharpe_CVGS.Controllers
{
    public class FriendshipController : Controller
    {
        /// <summary>
        /// Prop and Fields
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager;

        private ApplicationUser CurrentUser
        {
            get
            {
                return userManager.FindById(User.Identity.GetUserId());
            }
        }

        private bool IsEmployee
        {
            get
            {
                return db.Employees.Any(u => u.User == CurrentUser);
            }
        }

        private Member CurrentMember
        {
            get
            {
                return db.Members.FirstOrDefault(m => m.User.UserName == CurrentUser.UserName);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FriendshipController()
        {
            db = new ApplicationDbContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }




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

            var listUser = DummyUsers(list);

            Member currentMem = CurrentMember;

            var friends = db.Friendships.Where(a => a.FrienderId == currentMem.Id && a.IsFamilyMember == false).ToList();
            var family = db.Friendships.Where(a => a.FrienderId == currentMem.Id && a.IsFamilyMember == true).ToList();

            ViewData.Add("friends", friends);
            ViewData.Add("family", family);

            //var friend3Incl = db.Friendships.Include(f => f.Friendee).Include(f => f.Friender);

            return View(friends);
        }

        List<ApplicationUser> DummyUsers(List<Friendship> li)
        {
            List<ApplicationUser> res = new List<ApplicationUser>();

            for (int i = 0; i < li.Count; i++)
            {
                res.Add(li[i].Friendee.User);
            }
            return res;
        }

            /// <summary>
        /// Creates Dummy list of friends
        /// </summary>
        /// <returns></returns>
        List<Friendship> DummyFriends()
        {
            List<Friendship> res = new List<Friendship>();

            for (int i = 1; i < 11; i++)
            {
                var friendSh = new Friendship();
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
            
            //res.User = new ApplicationUser();
            res.User = CurrentUser;
            res.Id = id;

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
