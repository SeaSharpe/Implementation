using SeaSharpe_CVGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeaSharpe_CVGS.Controllers
{
    public class Controller : System.Web.Mvc.Controller
    {
        ApplicationDbContext _db;

        public ApplicationDbContext db
        {
            get
            {
                return _db ?? new ApplicationDbContext();
            }

            set
            {
                _db = value;
            }
        }
    }
}