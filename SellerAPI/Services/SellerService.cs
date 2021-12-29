using SellerAPI.Models;
using SellerAPI.Repositories;
using System;
using System.Threading.Tasks;

namespace SellerAPI.Services
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _repository;
        public SellerService(ISellerRepository productRepository)
        {
            _repository = productRepository;
        }
        public async Task AddProduct(Product product)
        {
            product.Id = Guid.NewGuid().ToString();
            await _repository.AddProduct(product);
        }

        public async Task DeleteProduct(string productId)
        {
            await _repository.DeleteProduct(productId);
        }

        public Task<Product> GetProduct(string productId)
        {
            return _repository.GetProduct(productId);
        }
    }
}
