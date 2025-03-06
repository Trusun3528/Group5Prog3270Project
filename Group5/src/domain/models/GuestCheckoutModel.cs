using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Group5.src.domain.models
{
    public class GuestCheckoutModel
    {
        //guest can add their email just to track their cart
        public string? GuestEmail { get; set; } 

        //this is just because the guets can only have access to their own cart and their own cart only
        public List<GuestCartItemModel> CartItems { get; set; } = new List<GuestCartItemModel>();
    }

    public class GuestCartItemModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
