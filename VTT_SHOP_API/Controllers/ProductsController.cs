using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using VTT_SHOP_CORE.DTOs;
using VTT_SHOP_CORE.Services;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {
        private readonly ProductService _product;

        public ProductsController(ProductService product)
        {
            _product = product;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            var result = await _product.GetProductByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(new { Message = result.Errors.FirstOrDefault()?.Message });
        }

        [HttpGet("search-product")]
        public async Task<IActionResult> SearchProduct([FromQuery] string name)
        {
            var result = await _product.SearchProductByNameAsync(name);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(new { Message = result.Errors.FirstOrDefault()?.Message });
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct(CreateProductDTO productDTO)
        {
            var result = await _product.AddProductAsync(productDTO);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetProductById), new { id = result.Value.Id }, result.Value);
            }
            return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO productDTO)
        {
            var result = await _product.UpdateProductAsync(productDTO);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var result = await _product.DeleteProductAsync(id);
            if (result.IsSuccess)
            {
                return Ok(new { Message = result.Successes.FirstOrDefault()?.Message });
            }
            return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
        }

        [HttpGet("filter-by-price")]
        public async Task<IActionResult> FilterByPrice([FromQuery] decimal priceMin, [FromQuery] decimal priceMax)
        {
            var result = await _product.FilterProductByPriceAsync(priceMin, priceMax);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(new { Message = result.Errors.FirstOrDefault()?.Message });
        }
    }
}