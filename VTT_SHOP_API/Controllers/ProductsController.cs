using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_CORE.Services;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductPagingParams pagingParams)
        {
            var result = await _product.GetProductsPagedAsync(pagingParams);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            var result = await _product.GetProductByIdAsync(id);
            return HandleResult(result);
        }


        [HttpGet("search-product")]
        public async Task<IActionResult> SearchProduct([FromQuery] string name)
        {
            var result = await _product.SearchProductByNameAsync(name);
            return HandleResult(result);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct(CreateProductDTO productDTO)
        {
            var result = await _product.AddProductAsync(productDTO);
            return HandleResult(result);
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO productDTO)
        {
            var result = await _product.UpdateProductAsync(productDTO);
            return HandleResult(result);
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var result = await _product.DeleteProductAsync(id);
            return HandleResult(result);
        }

        [HttpGet("filter-by-price")]
        public async Task<IActionResult> FilterByPrice([FromQuery] decimal priceMin, [FromQuery] decimal priceMax)
        {
            var result = await _product.FilterProductByPriceAsync(priceMin, priceMax);
            return HandleResult(result);
        }

        [HttpGet("get-product-picture")]
        public async Task<IActionResult> GetProductPictures(long productId)
        {
            var result = await _product.GetProductPicturesAsync(productId);
            return HandleResult(result);
        }

        [HttpPost("add-product-picture")]
        public async Task<IActionResult> AddProductPicture(UpdateProductPictureDTO pictureDTO)
        {
            var result = await _product.AddProductPictureAsync(pictureDTO);
            return HandleResult(result);
        }

        [HttpDelete("delete-product-picture/{pictureId}")]
        public async Task<IActionResult> DeleteProductPicture(long productId, long pictureId)
        {
            var result = await _product.DeleteProductPictureAsync(productId, pictureId);
            return HandleResult(result);
        }
        [HttpPut("set-main-product-picture")]
        public async Task<IActionResult> SetMainProductPicture(long productId, long pictureId)
        {
            var result = await _product.SetMainProductPictureAsync(productId, pictureId);
            return HandleResult(result);
        }

        [HttpPut("un-set-main-product-picture")]
        public async Task<IActionResult> UnSetMainProductPicture(long productId, long pictureId)
        {
            var result = await _product.UnsetMainProductPictureAsync(productId, pictureId);
            return HandleResult(result);
        }
    }
}