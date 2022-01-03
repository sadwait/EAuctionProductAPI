using SellerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellerAPI.Services
{
    public interface ISellerService
    {
        Task<List<Product>> GetAllProducts();

        Task<BidsDetails> GetAllBidsWithProductInfo(string productId);

        Task AddProduct(Product product);

        Task DeleteProduct(string productId);
    }
}
