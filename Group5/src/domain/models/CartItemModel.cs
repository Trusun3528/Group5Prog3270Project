/*
 * Project: Open Source Web Programing Midterm Check-In
 * Group Number: 5
 * Group Members: Patrick Harte, Austin Casselman, Austin Cameron, Leif Johannesson
 * Revision History:
 *      Created: January 25th, 2025
 *      Submitted: March 6th, 2025
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public Cart? Cart { get; set; }
        public Product? Product { get; set; }
    }
}
