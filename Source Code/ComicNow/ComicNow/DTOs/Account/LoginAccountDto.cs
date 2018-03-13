using System.ComponentModel.DataAnnotations;

namespace ComicNow.DTOs.Account
{
    public class LoginAccountDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}