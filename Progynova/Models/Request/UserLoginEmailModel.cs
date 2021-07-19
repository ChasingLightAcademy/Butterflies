using System.ComponentModel.DataAnnotations;

namespace Progynova.Models.Request
{
    public class UserLoginEmailModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Recaptcha { get; set; }
    }
}