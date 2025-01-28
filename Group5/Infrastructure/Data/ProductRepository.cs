using Application.Interfaces;
using System.Collections.Generic;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Boot", Price = 35.99m },
            new Product { Id = 2, Name = "Shirt", Price = 19.99m }
        };

        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }
    }
}
