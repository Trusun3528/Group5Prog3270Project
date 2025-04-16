using System.ComponentModel.DataAnnotations;

namespace Group5.src.domain.models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Role { get; set; }

        public int addressId { get; set; }


        public ICollection<Card>? Cards { get; set; }//collection of cards for having multiple cards
        public ICollection<Cart>? Carts { get; set; }//collection of cards for having multiple carts
        public ICollection<Order>? Orders { get; set; }//collection of cards for having multiple orders

    }
}
