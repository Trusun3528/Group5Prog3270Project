using System.ComponentModel.DataAnnotations;

namespace Group5.src.domain.models
{
    public class Rating
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string? UserId { get; set; }

        [Range(1, 5)]
        public double RatingNumber { get; set; }
        public string? Comment { get; set; }
        
    }
}
