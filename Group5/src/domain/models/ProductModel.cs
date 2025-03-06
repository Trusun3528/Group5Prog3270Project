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

namespace Group5.src.domain.models;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string? ProductName { get; set; }
    [Column(TypeName = "decimal(18,2)")]//money format
    public double? Price { get; set; }
    [MaxLength(400)]
    public string? ProductDescription { get; set; }
    [Required]
    public int Stock { get; set; }
    [MaxLength(50)]
    public string? Catagory { get; set; }
    [MaxLength(500)]
    public string? ImageURL { get; set; }

}
