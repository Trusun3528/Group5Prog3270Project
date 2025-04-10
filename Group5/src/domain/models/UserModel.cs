using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Group5.src.domain.models
{
    public class User : IdentityUser
    {
        public ICollection<Card>? Cards { get; set; }//collection of cards for having multiple cards
        public ICollection<Cart>? Carts { get; set; }//collection of cards for having multiple carts
        public ICollection<Order>? Orders { get; set; }//collection of cards for having multiple orders
    }
}
