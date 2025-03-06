/*
 * Project: Open Source Web Programing Midterm Check-In
 * Group Number: 5
 * Group Members: Patrick Harte, Austin Casselman, Austin Cameron, Leif Johannesson
 * Revision History:
 *      Created: January 25th, 2025
 *      Submitted: March 6th, 2025
 */

using System.ComponentModel.DataAnnotations;

namespace Group5.src.domain.models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(60)]
        public string? Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Role { get; set; }

        public int addressId { get; set; }


        public ICollection<Card>? Cards { get; set; }//collection of cards for having multiple cards
        public ICollection<Cart>? Carts { get; set; }//collection of cards for having multiple carts
        public ICollection<Order>? Orders { get; set; }//collection of cards for having multiple orders
    }
}
