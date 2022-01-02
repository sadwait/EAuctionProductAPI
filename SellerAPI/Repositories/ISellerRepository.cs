using SellerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellerAPI.Repositories
{
    public interface ISellerRepository
    {
        Task<Product> GetProduct(string productId);

        Task AddProduct(Product product);

        Task DeleteProduct(string productId);

        Task<List<Bids>> GetAllBidsByProductId(string productId);
    }
}
