using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_CORE.DTOs;
using VTT_SHOP_CORE.Services;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _product;

        public ProductsController(ProductService product)
        {
            _product = product;
        }
        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProduct(string name)
        {
            var products = await _product.SearchProductByNameAsync(name);
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductDTO productDTO)
        {
            var product = await _product.AddProductAsync(productDTO);
            if (product == null)
            {
                return BadRequest();
            }
            return Ok(product);
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(UpdateProductDTO productDTO)
        {
            var product = _product.UpdateProduct(productDTO);
            if (product == null)
            {
                return BadRequest();
            }
            return Ok(product);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productIsDelete = await _product.DeleteProductAsync(id);
            if (productIsDelete == false)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("FilterByPrice")]
        public async Task<IActionResult> FilterByPrice(double priceMin, double priceMax)
        {
            var products = await _product.FilterProductByPriceAsync(priceMin, priceMax);
            if (products.Count == 0) 
            {
                return NotFound();
            }
            return Ok(products);
        }
          

    }
}
