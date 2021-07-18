using Progynova.Enums;

namespace Progynova.Models.Common
{
    public class UserModel
    {
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string Email { get; set; }
        
        public string Bio { get; set; }
        
        public UserPermission Permission { get; set; }
    }
}