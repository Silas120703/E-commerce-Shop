using AutoMapper;
using FluentResults;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(ProductRepository product, IMapper mapper, IUnitOfWork unitOfWork) : base(product)
        {
            _product = product;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(long Id)
        {
            var product = await _product.GetProductByIdAsync(Id);
            if (product == null)
            {
                return Result.Fail("No products found");
            }
            return Result.Ok(_mapper.Map<ProductDTO>(product));
        }

        public async Task<Result<ProductDTO>> AddProductAsync(CreateProductDTO productDTO)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var product = _mapper.Map<Product>(productDTO);
                var newProduct = await _product.AddProductAsync(product);

                await _unitOfWork.SaveChangesAsync();

                var updatedProduct = _product.AddSlugName(newProduct);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return Result.Ok(_mapper.Map<ProductDTO>(updatedProduct))
                             .WithSuccess("Create successful products");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail("An error occurred while adding the product.: " + ex.Message);
            }
        }

        public async Task<Result<ProductDTO>> UpdateProductAsync(UpdateProductDTO productDTO)
        {
            try
            {
                var existingProduct = await _product.GetByIdAsync(productDTO.Id);
                if (existingProduct == null)
                {
                    return Result.Fail($"No product found with Id {productDTO.Id}");
                }

                _mapper.Map(productDTO, existingProduct);

                var updatedProduct = _product.AddSlugName(existingProduct);

                await _unitOfWork.SaveChangesAsync();
                return Result.Ok(_mapper.Map<ProductDTO>(updatedProduct))
                             .WithSuccess("Product update successful");
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while updating the product: " + ex.Message);
            }
        }

        public async Task<Result<List<ProductDTO>>> SearchProductByNameAsync(string name)
        {
            var products = await _product.SearchProductByNameAsync(name);
            if (products == null || !products.Any())
            {
                return Result.Fail("No products found");
            }
            return Result.Ok(_mapper.Map<List<ProductDTO>>(products));
        }

        public async Task<Result<List<ProductDTO>>> FilterProductByPriceAsync(decimal priceMin, decimal priceMax)
        {
            var products = await _product.FilterProductByPriceAsync(priceMin, priceMax);
            if (products == null || !products.Any())
            {
                return Result.Fail("No products found in this price range");
            }
            return Result.Ok(_mapper.Map<List<ProductDTO>>(products));
        }

        public async Task<Result> DeleteProductAsync(long id)
        {
            try
            {
                var product = await _product.GetByIdAsync(id);
                if (product == null)
                {
                    return Result.Fail("No products found");
                }

                _product.Delete(product);

                await _unitOfWork.SaveChangesAsync(); 

                return Result.Ok().WithSuccess("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while deleting the product: " + ex.Message);
            }
        }
    }
}