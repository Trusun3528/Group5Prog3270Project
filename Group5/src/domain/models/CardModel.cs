using System.ComponentModel.DataAnnotations;

namespace Group5.src.domain.models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }
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
