using System.ComponentModel.DataAnnotations;

namespace SycamoreCommercial.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
        [Required]
      
        [Range(0, 50, ErrorMessage = "Display Order must be between 1-100")]
        public int Order { get; set; }
    }
}
