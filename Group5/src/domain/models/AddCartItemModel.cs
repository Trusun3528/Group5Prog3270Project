using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Group5.src.domain.models
{
    public class AddCartItemModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
