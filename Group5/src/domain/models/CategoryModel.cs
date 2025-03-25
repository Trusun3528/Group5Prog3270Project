using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Group5.src.domain.models
    
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        [Required]
        public string? Description { get; set; }

    }
}
