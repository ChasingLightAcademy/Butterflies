using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Progynova.DbModels
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        
        public virtual int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        
        public virtual List<User> Selectee { get; set; }
        
        public virtual List<Lesson> Lessons { get; set; }
    }
}