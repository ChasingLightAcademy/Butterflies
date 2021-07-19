using Progynova.Enums;

namespace Progynova.Models.Common
{
    public class UserInfoModel
    {
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string Email { get; set; }
        
        public string Nickname { get; set; }
        
        public string Bio { get; set; }
        
        public UserPermission Permission { get; set; }
    }
}