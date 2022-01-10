using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SellerAPI.Models;
using SellerAPI.Services;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SellerAPI.Controllers
{
    [Route("e-auction/api/v1/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _productService;
        private readonly ILogger<SellerController> _logger;
        public SellerController(ISellerService productService, ILogger<SellerController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("get-products")]     
        public async Task<ActionResult> GetProducts()
        {
            _logger.LogInformation("Start fetching Products");

            var result = await _productService.GetAllProducts();
            return Ok(result);
        }

        [HttpGet]
        [Route("show-bids/{productId}")]
        public async Task<ActionResult> Get(string productId)
        {
            _logger.LogInformation("Start fetching Products by id:" + productId);

            var result = await _productService.GetAllBidsWithProductInfo(productId);
            return Ok(result);
        }

        [HttpPost]
        [Route("add-product")]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            await _productService.AddProduct(product);
            return Created("add-product", product);
        }

        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            await _productService.DeleteProduct(productId);
            return NoContent();
        }
    }
}
