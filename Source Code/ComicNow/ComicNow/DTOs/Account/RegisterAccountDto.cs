using System.ComponentModel.DataAnnotations;

namespace ComicNow.DTOs.Account
{
    public class RegisterAccountDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Email { get; set; }
    }
}