using SellerAPI.Models;
using System.Threading.Tasks;

namespace SellerAPI.Services
{
    public interface ISellerService
    {
        Task<Product> GetProduct(string productId);
        Task AddProduct(Product product);
        Task DeleteProduct(string productId);
    }
}
