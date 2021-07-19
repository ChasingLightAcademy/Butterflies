using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Progynova.Enums;

namespace Progynova.DbModels
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [RegularExpression("/^[a-zA-Z0-9_-]*/")]
        [MinLength(6)]
        [MaxLength(20)]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [MinLength(2)]
        [MaxLength(50)]
        public string Nickname { get; set; }
        
        [MaxLength(150)]
        public string Bio { get; set; }
        
        [Required]
        public UserPermission Permission { get; set; }
        
        public virtual List<Course> SelectedCourses { get; set; }

        public virtual List<Course> CreatedCourses { get; set; }
    }
}