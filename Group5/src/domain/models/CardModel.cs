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
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        [MaxLength(16)]
        public string? CreditCardNumber { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        [MaxLength(3)]
        public string? CVV { get; set; }
    }
}
