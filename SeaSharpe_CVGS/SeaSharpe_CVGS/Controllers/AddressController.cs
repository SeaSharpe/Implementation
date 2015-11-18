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
    public class AddressController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Partial view in member profile
        /// </summary>
        /// <returns>Partial view of address forms</returns>
        public ActionResult PartialCreate()
        {
            return View();
        }

        /// <summary>
        /// post back for create address
        /// </summary>
        /// <param name="address">Address object</param>
        /// <returns>****Unknown for partial views at this time****</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,StreetAddress,City,Region,Country,PostalCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(address);
        }

        /// <summary>
        /// Partial view in member profile
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>Partial view on profile page</returns>
        public ActionResult PartialEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        /// <summary>
        /// postback for address update
        /// </summary>
        /// <param name="address">address object</param>
        /// <returns>**unknown for partial views at this time***</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,StreetAddress,City,Region,Country,PostalCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(address);
        }

        
        /// <summary>
        /// Delete an address by clearing all fields
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>***unknown for partial views at this time***</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
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
    }
}
