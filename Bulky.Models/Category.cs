using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //[DisplayName("Category Order")]
        [MaxLength(20)]
        public string Name { get; set; }
        //[DisplayName("Display Order")]
        [Range(1, 100)]
        public int DiplayOrder { get; set; }
    }
}
