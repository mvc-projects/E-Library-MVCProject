using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCProjectVIdent.ViewModels
{
    public class UsersWithRuleViewModel
    {
        public string UserId { get; set; }
        [Required]
        public string fName { get; set; }
        [Required]
        public string lName { get; set; }
        [Required]
        public string Username { get; set; }
        //[Required]
        //[EmailAddress]
        //[Display(Name = "E-mail")]
        //public string Email { get; set; }
        [Required]
        [Display(Name = "Position")]
        public string Role { get; set; }

        [Required]
        [Display(Name = "Salary")]
        public decimal salary { get; set; }

        [Required]
        [Display(Name = "Hire Date")]
        public System.DateTime JoinDate { get; set; }
    }
}