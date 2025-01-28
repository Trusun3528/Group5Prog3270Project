using Application.Interfaces;
using System.Collections.Generic;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10.0m },
            new Product { Id = 2, Name = "Product 2", Price = 20.0m }
        };

        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }
    }
}
