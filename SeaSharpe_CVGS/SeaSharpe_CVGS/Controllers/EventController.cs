﻿using System;
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
    /// <summary>
    /// Controller class for handling actions related to events
    /// </summary>
    public class EventController : Controller
    {
        #region Multiple Roles
        /// <summary>
        /// checks authorization and redirects to appropriate page
        /// </summary>
        /// <returns>redirect to event management or view events methods</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult Index()
        {
            return RedirectToAction("EventManagement");
        }
        
        
        #endregion
        #region Employee Side
        /// <summary>
        /// list all events 
        /// </summary>
        /// <returns>Event Management view</returns>
        [Authorize(Roles = "Employee")]
        public ActionResult EventManagement()
        {
            return View(db.Events.ToList());
        }

        /// <summary>
        /// Employee side - add events
        /// </summary>
        /// <returns>Add/Edit Event view</returns>
        [Authorize(Roles = "Employee")]
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
            try
            {
                @event.Employee = CurrentEmployee;
                ModelState.Clear();
                if (TryValidateModel(@event))
                {
                    db.Events.Add(@event);
                    db.SaveChanges();
                    TempData["message"] = CurrentEmployee.User.FirstName + " " + CurrentEmployee.User.LastName +
                       ": " + @event.Description + " has been created.";
                    return RedirectToAction("EventManagement");
                }
            }
            catch (Exception e)
            {
                TempData["message"] = "Error creating event: " + e.GetBaseException().Message;
            }

           return View(@event);
        }

       /// <summary>
       /// Employee side - edit event
       /// </summary>
       /// <param name="id">event id</param>
       /// <returns>Add/Edit Event view</returns>
       [Authorize(Roles = "Employee")]
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
           try
           {
               // Get the current Employee
               @event.Employee = CurrentEmployee;
               ModelState.Clear();
               if (ModelState.IsValid)
               {
                   db.Entry(@event).State = EntityState.Modified;
                   db.SaveChanges();
                   TempData["message"] = CurrentEmployee.User.FirstName + " " + CurrentEmployee.User.LastName +
                       ": " + @event.Description + " has been updated.";
                   return RedirectToAction("EventManagement");
               }
           }
           catch (Exception e)
           {
               TempData["message"] = "Error updating event: " + e.GetBaseException().Message;
           }

           return View(@event);
       }

       /// <summary>
       /// Employee -side post back for delete event.  **no delete view, delete button on list of events view***
       /// ****No view required****
       /// </summary>
       /// <param name="id">event id</param>
       /// <returns>list of events view</returns>
       [Authorize(Roles = "Employee")]
        public ActionResult Delete(int id)
        {
           try
           {
               // Find the event
               Event @event = db.Events.Find(id);

               // First delete any users that are part of the event, then delete the event
               if (@event.Attendies.Count() > 0)
               {
                   // delete the members from the event
                   @event.Attendies.Clear();
               }
           
               // Delete the event and save changes to the database
               db.Events.Remove(@event);
               db.SaveChanges();
               TempData["message"] = CurrentEmployee.User.FirstName + " " + CurrentEmployee.User.LastName + 
                   ": " + @event.Description + " has been deleted.";
           }
           catch (Exception e)
           {
               TempData["message"] = "Error deleting event: " + e.GetBaseException().Message;
           }

           return RedirectToAction("Index");
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
        /// Member Side - register for event
        /// **** No View Required ****
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Member")]
        public ActionResult Register(int? id)
        {
            // Find the current event by id
            var currentEvent = db.Events.Find(id);
            // Store the current event's capacity
            int eventCapacity = currentEvent.Capacity;
            // Determine how many members are currently attending the current event
            int currentAttendies = currentEvent.Attendies.Count();

            // Check to see if the member is already attending the event
            if ( currentEvent.Attendies.Any(m => m.Id == CurrentMember.Id))
            {
                TempData["message"] = CurrentMember.User.FirstName + " " +
                    CurrentMember.User.LastName + 
                    ", you are already attending " +
                    currentEvent.Description;
            }
            // Check to see if the event has any capcacity left
            else if (currentAttendies >= eventCapacity)
            {
                TempData["message"] = "Sorry, " + currentEvent.Description +
                    " is currently full";
            }
            // If the member is not already attending the event and the event has capacity
            // let the member join the event
            else
            {
                CurrentMember.Events.Add(db.Events.Find(id));
                db.SaveChanges();
                TempData["message"] = CurrentMember.User.FirstName + " " +
                    CurrentMember.User.LastName + " you have now succesfully joined " +
                    currentEvent.Description;
            }
            
            return RedirectToAction("ViewEvents");
        }
        #endregion

    }
}
