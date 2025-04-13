using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Group5.src.domain.models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CartID { get; set; }
        [Required]
        public int ProductID { get; set; } 
        [Required]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double? Price { get; set; }

        [JsonIgnore]
        public Cart? Cart { get; set; }
        public Product? Product { get; set; }
    }
}
