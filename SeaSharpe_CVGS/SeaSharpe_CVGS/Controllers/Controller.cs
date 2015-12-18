using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SeaSharpe_CVGS.Models;

namespace SeaSharpe_CVGS.Controllers
{
    /// <summary>
    /// This class gives some common functionality to all of our controllers
    /// </summary>
    public class Controller : System.Web.Mvc.Controller
    {
        ApplicationDbContext _db;
        ApplicationSignInManager _signInManager;
        ApplicationUserManager _userManager;
        
        /// <summary>
        /// Gets or creates a reference to the application context
        /// </summary>
        public ApplicationDbContext db
        {
            get
            {
                return _db ?? (_db = new ApplicationDbContext());
            }

            protected set
            {
                _db = value;
            }
        }

        /// <summary>
        /// Gets or creates a reference to the application context
        /// </summary>
        public ApplicationDbContext DbContext
        {
            get
            {
                return _db ?? (_db = new ApplicationDbContext());
            }

            set
            {
                _db = value;
            }
        }

        /// <summary>
        /// Gets or creates a reference to the SignInManager
        /// </summary>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            protected set
            {
                _signInManager = value;
            }
        }

        /// <summary>
        /// Gets or creates a reference to the UserManager
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            protected set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Gets the user that is currently logged in, null if logged out
        /// </summary>
        public ApplicationUser CurrentUser
        {
            get
            {
                return DbContext.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            }
        }

        /// <summary>
        /// Gets the member that is currently logged in, null if logged out or not a member
        /// </summary>
        public Member CurrentMember
        {
            get
            {
                return DbContext.Members.FirstOrDefault(m => m.User.UserName == User.Identity.Name);
            }
        }

        /// <summary>
        /// Gets the employee that is currently logged in, null if logged out or not an employee
        /// </summary>
        public Employee CurrentEmployee
        {
            get
            {
                return DbContext.Employees.FirstOrDefault(e => e.User.UserName == User.Identity.Name);
            }
        }

        /// <summary>
        /// Check if a user is logged in
        /// </summary>
        public bool IsAuthenticated { get { return User.Identity.IsAuthenticated; } }

        /// <summary>
        /// Check if a user is an employee
        /// </summary>
        public bool IsEmployee { get { return CurrentEmployee != null; } }

        /// <summary>
        /// Check if a user is a member
        /// </summary>
        public bool IsMember { get { return CurrentMember != null; } }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources 
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources;
        /// false to release only unmanaged resources</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }

                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}