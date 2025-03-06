/*
 * Project: Open Source Web Programing Midterm Check-In
 * Group Number: 5
 * Group Members: Patrick Harte, Austin Casselman, Austin Cameron, Leif Johannesson
 * Revision History:
 *      Created: January 25th, 2025
 *      Submitted: March 6th, 2025
 */

namespace Group5.src.domain.models
{
    public class Address
    {
        public int id { get; set; }
        public string? customerName { get; set; }
        public string? city { get; set; }
        public string? street { get; set; }
        public string? state { get; set; }
        public string? postalCode { get; set; }

        public string? country { get; set; }



    }
}
