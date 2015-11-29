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
            Member member = db.Members.Find(id);

            if (id == null)
            {   // Return view with current member when no id supplied
                return View(CurrentMember);
            }
            else if (CurrentMember != null && CurrentMember.Id == id)
            {   // Return view with current member when their id supplied
                return View(CurrentMember);
            }

            if (IsEmployee)
            {   
                if (member != null)
                {
                    return View(member);
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return new HttpUnauthorizedResult("You do not have permission to edit another member's profile.");
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
