using Newtonsoft.Json;
using SellerAPI.Common;
using SellerAPI.MessageBroker;
using SellerAPI.Models;
using SellerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellerAPI.Services
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _repository;
        private readonly IRabbitMqListener _rabbitMqListener;
        private readonly ICacheService _cacheService;

        public SellerService(ISellerRepository productRepository, IRabbitMqListener rabbitMqListener, ICacheService cacheService)
        {
            _repository = productRepository;
            _rabbitMqListener = rabbitMqListener;
            _cacheService = cacheService;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return  await _repository.GetAllProducts();            
        }

        public async Task<BidsDetails> GetAllBidsWithProductInfo(string productId)
        {
            var bidsDetails = new BidsDetails();
            //Reading from Redis Cache
            var prodInfo = await _cacheService.Get<Product>(productId);
            if (prodInfo == null)
            {
                prodInfo = await _repository.GetProduct(productId);
                if (prodInfo != null)
                    await _cacheService.Set<Product>(productId, JsonConvert.SerializeObject(prodInfo));
            }
            bidsDetails.ProductInfo = prodInfo;
            bidsDetails.BidsList = await _repository.GetAllBidsByProductId(productId);

            //Read Rabbitmq queue
            _rabbitMqListener.Receive();

            return bidsDetails;
        }

        public async Task AddProduct(Product product)
        {
            if (product.BidEndDate <= DateTime.Now)
            {
                throw new ArgumentException("Bid End Date should be future date");
            }
            else if (!Enum.IsDefined(typeof(ProductCategory), product.Category))
            {
                throw new ArgumentException("Product Category should be the one from the existing - (Painting, Sculptor, Ornament)");
            }
            product.Id = Guid.NewGuid().ToString();
            await _repository.AddProduct(product);
        }

        public async Task DeleteProduct(string productId)
        {
            var product = await _repository.GetProduct(productId);
            if(product!=null)
            {
                if(product.BidEndDate<DateTime.Now)
                {
                    throw new ArgumentException("Product cannot be deleted after the bid end date");
                }
            }
            await _repository.DeleteProduct(productId);
        }
    }
}
