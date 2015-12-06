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
using System.Data.Entity.Validation;
using System.Text;

namespace SeaSharpe_CVGS.Controllers
{
    /// <summary>
    /// This controller handles user profile management, generic user managment is handled 
    /// in <see cref="SeaSharpe_CVGS.Controllers.AccountController"/>.
    /// </summary>
    [Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// Displays an edit page for updating a profile. Employees can specify a member 
        /// id to modify a member's profile, if left out the currently logged in member id
        /// will be used.
        /// </summary>
        /// <param name="id">member id (Optional)</param>
        /// <returns>Edit view</returns>
        public ActionResult Edit(int? id)
        {
            ProfileViewModel model = new ProfileViewModel
            {
                Member = db.Members.FirstOrDefault(m => m.Id == id || m.User.UserName == User.Identity.Name)
            };

            if (model.Member == null)
            {
                return HttpNotFound();
            }

            var memberAddresses = db.
                Addresses.
                Where(a => a.Member.Id == model.Member.Id).
                OrderBy(a => a.Id);

            // Billing address is the first address Shipping adddress is the second address or a new adress
            model.BillingAddress = memberAddresses.FirstOrDefault() 
                ?? new Address { Member = model.Member };
            model.ShippingAddress = memberAddresses.Skip(1).FirstOrDefault() 
                ?? new Address { Member = model.Member };

            if ( User.IsInRole("Employee") || model.Member.User.UserName == User.Identity.Name )
            {
                return View(model);
            }

            throw new UnauthorizedAccessException("You may not access this profile.");
        }

        /// <summary>
        /// Post back method for the profile page
        /// </summary>
        /// <param name="member">The member object</param>
        /// <param name="billingAddress">The member's shipping address</param>
        /// <param name="shippingAddress">The member's billing address</param>
        /// <returns>Returns to index if successful, otherwise redisplays the 
        /// edit page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Prefix = "Member")] Member member, 
            [Bind(Prefix = "BillingAddress")] Address billingAddress, 
            [Bind(Prefix = "ShippingAddress")] Address shippingAddress)
        {
            var model = new ProfileViewModel { Member = member, BillingAddress = billingAddress, ShippingAddress = shippingAddress };
            var sqlLog = new StringBuilder("");
            db.Database.Log = x => sqlLog.Append(x);

            foreach (var address in new Address[] { billingAddress, shippingAddress })
            {
                if (address != null) 
                {
                    address.Member = member;
                    if (address.Id == 0)
                    {
                        if (!String.IsNullOrWhiteSpace(address.StreetAddress) &&
                            !String.IsNullOrWhiteSpace(address.Region) &&
                            !String.IsNullOrWhiteSpace(address.City) &&
                            !String.IsNullOrWhiteSpace(address.Country) &&
                            !String.IsNullOrWhiteSpace(address.PostalCode))
                        {
                            db.Addresses.Add(address);
                        }
                    }
                    else
                    {
                        db.Addresses.Attach(address);
                        db.Entry<Address>(address).State = EntityState.Modified;
                    }
                }
            }

            db.Members.Attach(member);

            if (TryValidateModel(model))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home")
                }
                catch (DbEntityValidationException e)
                {
                    StringBuilder sb = new StringBuilder("");
                    foreach (var entity in e.EntityValidationErrors)
                    {
                        foreach (var error in entity.ValidationErrors)
                        {
                            sb.Append(string.Format("{0} -> {1}\n", error.PropertyName, error.ErrorMessage));
                        }
                    }
                    // Display errors
                    TempData["message"] = sb.ToString();
                }
            }

            // Return to view if db not updated.
            return View(model);
        }
    }
}
