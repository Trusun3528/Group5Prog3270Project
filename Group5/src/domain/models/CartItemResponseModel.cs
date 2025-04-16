using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Group5.src.domain.models
{
    public class CartItemResponse
    {
        public Product? Product { get; set; }
        public int Quantity { get; set; }
    }
}
