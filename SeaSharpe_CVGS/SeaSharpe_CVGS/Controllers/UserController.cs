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
    public class UserController : Controller
    {
        #region Member Side
        /// <summary>
        /// Displays sign up form for new members
        /// </summary>
        /// <returns>Create view</returns>
        public ActionResult Create()
        {
            return View();
        }

       /// <summary>
       /// post back for create member 
       /// </summary>
       /// <param name="member">member object</param>
       /// <returns>redirect to Home/Index(display message to check email for verification)</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,IsEmailVerified,IsEmailMarketingAllowed,StripeID")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Home/Index");
            }

            return View(member);
        }

        /// <summary>
        /// Displays member profile page
        /// ** contains (2) partial address views**
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>Edit view</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        /// <summary>
        /// post back for edit member
        /// </summary>
        /// <param name="member">member object</param>
        /// <returns>redirect to Game/SearchGames</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,IsEmailVerified,IsEmailMarketingAllowed,StripeID")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Game/SearchGames");
            }
            return View(member);
        }

        /// <summary>
        /// verifies that the user is using an email address that they have access to
        /// ***No view required***
        /// </summary>
        /// <param name="id">member id from emailed link</param>
        /// <param name="verificationHash">verification hash from emailed link</param>
        /// <returns>redirect to Home/Index with message about account activation</returns>
        public ActionResult Verify(int? id, string verificationHash)
        {
            return RedirectToAction("Home/Index");
        }

        /// <summary>
        /// accessed from profile page when member knows current password
        /// ***no view required***
        /// </summary>
        /// <param name="memberId">member id</param>
        /// <param name="oldPassword">user-entered old password(must match db)</param>
        /// <param name="newPassword">user-entered new password</param>
        /// <returns>Profile page</returns>
        public ActionResult ResetPassword(int? memberId, string oldPassword, string newPassword)
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// accessed from login header when user has forgotten their password and can't login
        /// ***No view required****
        /// </summary>
        /// <param name="memberEmail">user-entered email</param>
        /// <param name="generatedTemporaryPassword">generated temp pw</param>
        /// <returns>Home page with message to check email for temporary password</returns>
        public ActionResult ForgotPassword(string memberEmail, string generatedTemporaryPassword)
        {
            return RedirectToAction("Home/Index");

        }
        #endregion

        /// <summary>
        /// authenticates the user and then logs them in
        /// ***form in visitor header***
        /// </summary>
        /// <param name="email">email from user entry</param>
        /// <param name="hashedPassword">hashed password from user entry</param>
        /// <returns>login and redirect to Game/SearchGames</returns>
        public ActionResult Login(string email, string hashedPassword)
        {
            return RedirectToAction("Game/SearchGames");

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
    }
}
