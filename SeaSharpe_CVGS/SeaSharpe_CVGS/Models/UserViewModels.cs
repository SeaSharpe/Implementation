using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SeaSharpe_CVGS.Models
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required, MinLength(1), MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, MinLength(1), MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public System.DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                foreach (var item in items)
                {
                    item.Selected = Gender == value;
                }
                _gender = value;
            }
        }
        private string _gender;

        public static Dictionary<string, string> genders = new Dictionary<string, string>() { { "Male", "M" }, {"Female", "F"}, {"Other", "O"} };
        public IEnumerable<System.Web.Mvc.SelectListItem> items = 
            from gender in genders 
            select new System.Web.Mvc.SelectListItem 
            { Text = gender.Key, Value = gender.Value };
    }
}
