using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Group5.src.domain.models;

namespace Group5.src.domain.models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public User? User { get; set; }

        // the list for items in cart but might already be being handeled
        //public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Get the price of all games in the cart
        /*public double GetSubTotal()
        {
            double subTotal = 0;

            foreach (var item in CartItems)
            {
                subTotal += item.Price;
            }

            return subTotal;
        }*/

        public ICollection<CartItem>? CartItems { get; set; }
    }
}
