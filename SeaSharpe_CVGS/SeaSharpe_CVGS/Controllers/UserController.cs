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
        /// post back for edit member
        /// </summary>
        /// <param name="member">member object</param>
        /// <returns>redirect to Game/SearchGames</returns>
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
                    TempData["Message"] = sb.ToString();
                }
            }

            return View(model);
        }

        void UpdateMember(Member updateFrom, Member updateTo)
        {
            updateTo.User.FirstName = updateFrom.User.FirstName;
            updateTo.User.LastName = updateFrom.User.LastName;
            updateTo.User.Email = updateFrom.User.Email;
            updateTo.User.Gender = updateFrom.User.Gender;
            updateTo.IsEmailMarketingAllowed = updateFrom.IsEmailMarketingAllowed;
        }

        void UpdateAddress(Address updateFrom, Address updateTo)
        {
            updateTo.PostalCode = updateFrom.PostalCode;
            updateTo.Region = updateFrom.PostalCode;
            updateTo.StreetAddress= updateFrom.PostalCode;
        }
    }
}
