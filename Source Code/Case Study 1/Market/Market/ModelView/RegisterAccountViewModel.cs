using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.ModelView
{
    public class RegisterAccountViewModel
    {
        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Username { get; set; }

        [Required]
        [MaxLength(16)]
        [MinLength(6)]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Password { get; set; }
        
        [Display(Name = "Confirmation")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        public static readonly int NormalUserId = 1;
    }
}