/*
 * File Name: UserController.cs
 * 
 * Handles user profiles
 */

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
    [Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// Displays member profile page
        /// ** contains (2) partial address views**
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>Edit view</returns>
        public ActionResult Edit(int? id)
        {
            Member member = db.Members.FirstOrDefault(m => m.Id == id || m.User.UserName == User.Identity.Name);

            if (member == null)
            {
                return HttpNotFound();
            }

            if ( User.IsInRole("Employee") || member.User.UserName == User.Identity.Name )
            {
                return View(member);
            }

            throw new UnauthorizedAccessException("You may not access this profile.");
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
    }
}
