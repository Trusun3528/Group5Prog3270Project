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
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? UserId { get; set; }
        public string? GuestEmail { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]//money format
        public decimal TotalAmount { get; set; }
        public decimal DiscountApplied { get; set; }
        [MaxLength(100)]
        public string? ShippingAddress { get; set; }

        public User? User { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
