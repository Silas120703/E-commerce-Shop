using AutoMapper;
using VTT_SHOP_CORE.DTOs;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using VTT_SHOP_SHARED.Services;


namespace VTT_SHOP_CORE.Services
{
    public class ProductService : ServiceBase<Product>
    {
        private readonly ProductRepository _product;
        private readonly IMapper _mapper;

        public ProductService(ProductRepository product, IMapper mapper, IUnitOfWork unitOfWork) : base(product)
        { 
            _product = product;
            _mapper = mapper;
        }
        public async Task<ProductDTO> GetProductByIdAsync(long Id) 
        { 
            var product = await _product.GetProductByIdAsync(Id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> AddProductAsync(CreateProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            var newProduct =await _product.AddProductAsync(product);
            var newProduct1 = _product.AddSlugName(newProduct);
            return _mapper.Map<ProductDTO>(newProduct);
        }

        public ProductDTO UpdateProduct(UpdateProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            var updateProduct = _product.UpdateProduct(product);
            var updateProduct1 = _product.AddSlugName(updateProduct);
            return _mapper.Map<ProductDTO>(updateProduct1);
        }
        public async Task<List<ProductDTO>> SearchProductByNameAsync(string name)
        {
            var products = await _product.SearchProductByNameAsync(name);
            return _mapper.Map<List<ProductDTO>>(products);
        }
        

        public async Task<List<ProductDTO>> FilterProductByPriceAsync(decimal priceMin , decimal priceMax)
        {
            var products = await _product.FilterProductByPriceAsync(priceMin, priceMax);
            return _mapper.Map<List<ProductDTO>>(products);
        }
        public async Task<bool> DeleteProductAsync(long id)
                {
                    var product = await _product.GetByIdAsync(id);
                    if (product == null) return false;

                    _product.Delete(product);
                    return true;
                }
    }
}
