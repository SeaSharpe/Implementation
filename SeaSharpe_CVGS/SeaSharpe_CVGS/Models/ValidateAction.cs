using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeaSharpe_CVGS.Models
{
    public class ValidateAction
    {
        //will hold the temporary value of startDate for an event. 
        //Use it to compare it with end date of an event
        public static DateTime EventStartDate;
    }

    #region Event Entity

    /// <summary>
    /// Will make sure the value pass is not in the past
    /// </summary>
    public class DateNotInThePast : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt;
            bool parsed = DateTime.TryParse((string)value.ToString(), out dt);
            if (!parsed)
            {
                return false;
            }

            if (dt < DateTime.Today)
            {
                return false;
            }

            ValidateAction.EventStartDate = dt;
            return true;
        }
    }

    /// <summary>
    /// Will make sure End Date of an event is later than the Start Date (ValidateAction.EventStartDate)
    /// </summary>
    public class DateNotBeforeStartDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var db = new ApplicationDbContext();

            DateTime dt;
            bool parsed = DateTime.TryParse((string)value.ToString(), out dt);
            if (!parsed)
            {
                return false;
            }

            if (dt < ValidateAction.EventStartDate)
            {
                return false;
            }

            return true;
        }
    }

    #endregion
}