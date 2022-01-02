using SellerAPI.Models;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SellerAPI.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private Container container, buyerContainer;
        public SellerRepository(CosmosClient client, string databaseName, string containerName)
        {
            container = client.GetContainer(databaseName, containerName);
            buyerContainer = client.GetContainer(databaseName, "Buyers");
        }

        public async Task<Product> GetProduct(string productId)
        {
            // var response=   await container.ReadItemAsync<Product>("863ab1c4-5385-499f-b78e-183c9874ea1f", new PartitionKey(productId));
            //  return response.Resource;

            IQueryable<Product> queryable = container.GetItemLinqQueryable<Product>(true);
            queryable = queryable.Where(item => item.Id == productId);
            return await Task.FromResult(queryable.ToArray().FirstOrDefault());
        }

        public async Task AddProduct(Product product)
        {
            await container.CreateItemAsync(product, new PartitionKey(product.Id));
        }

        public async Task DeleteProduct(string productId)
        {
            await container.DeleteItemAsync<Seller>(productId, new PartitionKey(productId));
        }

        public async Task<List<Bids>> GetAllBidsByProductId(string productId)
        {
            IQueryable<Bids> queryable = buyerContainer.GetItemLinqQueryable<Bids>(true);
            queryable = queryable.Where(item => item.ProductId == productId);
            return await Task.FromResult(queryable.ToList());
        }
    }
}
