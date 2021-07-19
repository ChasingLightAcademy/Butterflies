using System.ComponentModel.DataAnnotations;

namespace Progynova.Models.Request
{
    public class UserRegisterModel
    {
        [Required]
        [RegularExpression("/^[a-zA-Z0-9_-]*/")]
        [MinLength(6)]
        [MaxLength(20)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Recaptcha { get; set; }
    }
}