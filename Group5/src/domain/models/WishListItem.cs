namespace Group5.src.domain.models
{
    public class WishListItem
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int ProductId { get; set; } 
        public Product? Product { get; set; } 
    }
}
