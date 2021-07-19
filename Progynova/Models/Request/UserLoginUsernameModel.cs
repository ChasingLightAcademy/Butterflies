using System.ComponentModel.DataAnnotations;

namespace Progynova.Models.Request
{
    public class UserLoginUsernameModel
    {
        [Required]
        [RegularExpression("/^[a-zA-Z0-9_-]*/")]
        [MinLength(6)]
        [MaxLength(20)]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Recaptcha { get; set; }
    }
}