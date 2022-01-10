using Moq;
using NUnit.Framework;
using SellerAPI.MessageBroker;
using SellerAPI.Repositories;
using SellerAPI.Services;
using System;
using System.Threading.Tasks;

namespace SellerAPITest
{
    public class Tests
    {
        private SellerService _sellerService;
        ProductStub productStub = new ProductStub();

        [SetUp]
        public void Setup()
        {
            var sellerRepositoryMock = new Mock<ISellerRepository>();            
            var rabbitMqListenerMock = new Mock<IRabbitMqListener>();
            var cacheServiceMock = new Mock<ICacheService>();

            sellerRepositoryMock.Setup(x => x.GetAllProducts()).Returns(productStub.GetAllProducts());
            _sellerService = new SellerService(sellerRepositoryMock.Object, rabbitMqListenerMock.Object, cacheServiceMock.Object);
            
        }

        [Test]
        public async Task GetAllProducts_Test()
        {
            var allproducts = await _sellerService.GetAllProducts();
            Assert.AreEqual("fsad6673Zuwewe", allproducts[0].Id);
            Assert.AreEqual("Chain", allproducts[0].ProductName);
            Assert.AreEqual("Ornaments", allproducts[0].Category);
        }

        [Test]
        public async Task AddProduct_Test_BidEndDate_Exception()
        {
            string expectedMsg = "Bid End Date should be future date";
            try
            {
                await _sellerService.AddProduct(productStub.ProductInfo);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedMsg, ex.Message);
            }
        }

        [Test]
        public async Task AddProduct_Test_Category_Exception()
        {
            string expectedMsg = "Product Category should be the one from the existing - (Painting, Sculptor, Ornament)";
            try
            {
                var prodInfo = productStub.ProductInfo;
                prodInfo.BidEndDate = new DateTime(2022, 01, 20);
                await _sellerService.AddProduct(prodInfo);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedMsg, ex.Message);
            }
        }

        [Test]
        public async Task AddProduct_Test()
        {
            var prodInfo = productStub.ProductInfo;
            prodInfo.BidEndDate = new DateTime(2022, 01, 20);
            prodInfo.Category = "Ornament";

            await _sellerService.AddProduct(prodInfo);
            Assert.Pass();
        }
    }
}