using Microsoft.AspNetCore.Mvc;
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
        public SellerController(ISellerService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-products")]     
        public async Task<ActionResult> GetProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        }

        [HttpGet]
        [Route("show-bids/{productId}")]
        public async Task<ActionResult> Get(string productId)
        {
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
