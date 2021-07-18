using System.ComponentModel.DataAnnotations;

namespace Progynova.Models.Request
{
    public class UserAuthModel
    {
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public string Email { get; set; }
        
        [Required]
        public string Recaptcha { get; set; }
    }
}