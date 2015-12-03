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
    [Authorize(Roles="Employee")]
    public class EventController : Controller
    {
        #region Multiple Roles
        /// <summary>
        /// checks authorization and redirects to appropriate page
        /// </summary>
        /// <returns>redirect to event management or view events methods</returns>
        public ActionResult Index()
        {
            //if (Roles.IsUserInRole(@"employee"))
            //{
            //    return View(db.Events.ToList());
            return RedirectToAction("EventManagement");
            //}
            //else if (Roles.IsUserInRole(@"member"))
            //{
            //    //return ViewEvents view
            //      return RedirectToAction("ViewEvents");
            //}
            //else
            //{
            //    //return ViewEvents view
            //      return RedirectToAction("ViewEvents");
            //}

        }
        
        
        #endregion
        #region Employee Side
        /// <summary>
        /// list all events 
        /// </summary>
        /// <returns>Event Management view</returns>
        public ActionResult EventManagement()
        {
            return View(db.Events.ToList());
        }

        /// <summary>
        /// Employee side - add events
        /// </summary>
        /// <returns>Add/Edit Event view</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Employee side - post back for add event
        /// </summary>
        /// <param name="event">event object</param>
        /// <returns>Event Management view</returns>
       [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Location,StartDate,EndDate,Description,Capacity")] Event @event)
        {
            @event.Employee = CurrentEmployee;
            ModelState.Clear();
            if (TryValidateModel(@event))
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("EventManagement");
            }

            return View(@event);
        }

       /// <summary>
       /// Employee side - edit event
       /// </summary>
       /// <param name="id">event id</param>
       /// <returns>Add/Edit Event view</returns>
       public ActionResult Edit(int? id)
       {
           if (id == null)
           {
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           }
           Event @event = db.Events.Find(id);
           if (@event == null)
           {
               return HttpNotFound();
           }
           return View(@event);
       }

        /// <summary>
        /// post back for edit event
        /// </summary>
        /// <param name="event">event object</param>
        /// <returns>Event Management view</returns>
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult Edit([Bind(Include = "Id,Location,StartDate,EndDate,Description,Capacity")] Event @event)
       {
           @event.Employee = CurrentEmployee;
            ModelState.Clear();
           if (ModelState.IsValid)
           {
               db.Entry(@event).State = EntityState.Modified;
               db.SaveChanges();
               return RedirectToAction("EventManagement");
           }
           return View(@event);
       }

       /// <summary>
       /// post back for delete event
       /// </summary>
       /// <param name="id">event id</param>
       /// <returns>Event Management view</returns>
       [HttpPost, ActionName("Delete")]
       [ValidateAntiForgeryToken]
       public ActionResult DeleteConfirmed(int id)
       {
           Event @event = db.Events.Find(id);
           db.Events.Remove(@event);
           db.SaveChanges();
           return RedirectToAction("EventManagement");
       }
        #endregion

        #region Member Side
       /// <summary>
       /// list all current events
       /// </summary>
       /// <returns>ViewEvents view</returns>
       
       [AllowAnonymous]
       public ActionResult ViewEvents()
       {
           return View(db.Events.ToList());
       }
        /// <summary>
        /// display details of currently selected event
        /// </summary>
        /// <returns>PartialSelectedEvent view</returns>
        public ActionResult PartialSelectedEvent()
       {
           return View();
       }
        /// <summary>
        /// Member Side - register for event
        /// **** No View Required ****
        /// </summary>
        /// <returns></returns>
        public ActionResult Register(int eventId, int memberId)
        {
            return RedirectToAction("ViewEvents");
        }
        #endregion

    }
}
