using SellerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellerAPITest
{
    public class ProductStub
    {
        public Product ProductInfo
        {
            get
            {
                return new Product()
                {
                    Id = "fsad6673Zuwewe",
                    ProductName = "Chain",
                    Category = "",
                    StartingPrice = 100,
                    BidEndDate = new DateTime(2022, 01, 01)
                };
            }
            set { }
        }
        public Task<List<Product>> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            products.AddRange(new Product[] {
                new Product()
                {
                    Id="fsad6673Zuwewe",
                    ProductName ="Chain",
                    Category="Ornaments",
                    StartingPrice=100,
                    BidEndDate = new DateTime(2022,01,20)
                }
            });

            return Task.FromResult(products);
        }

    }
}
