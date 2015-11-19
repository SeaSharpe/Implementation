using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SeaSharpe_CVGS.Models
{
    public class ValidateAttributes
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

            ValidateAttributes.EventStartDate = dt;
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

            if (dt < ValidateAttributes.EventStartDate)
            {
                return false;
            }

            return true;
        }
    }

    #endregion

    public class PostalCodeValidation : ValidationAttribute
    {
        /// <summary>
        /// Evaluates if object passed match the regular expresion
        /// </summary>
        /// <param name="value">Object to be evaluated</param>
        /// <returns>True if the postal code is null or whitespaces, 
        /// True if the regular expression match the object value, false otherwise</returns>
        public override bool IsValid(object value)
        {
            string postalCode = "";
            if (value != null)
            {
                postalCode = value as string;
                postalCode = postalCode.Trim();

                if (string.IsNullOrWhiteSpace(postalCode))
                {
                    return true;
                }
                else
                {
                    return Regex.IsMatch
                        (postalCode, @"^(?i)[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] ?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$");
                }
            }
            //so will let if be optional
            else
            {
                return true;
            }
        }
    }
}