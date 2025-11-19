using AutoMapper;
using FluentResults;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using VTT_SHOP_SHARED.Services;
using VTT_SHOP_CORE.Errors;
using Microsoft.EntityFrameworkCore;
using VTT_SHOP_SHARED.Extensions;

namespace VTT_SHOP_CORE.Services
{
    public class ProductService : ServiceBase<Product>
    {
        private readonly ProductRepository _product;
        private readonly ProductPictureRepository _productPicture;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(ProductRepository product, ProductPictureRepository productPicture, IMapper mapper, IUnitOfWork unitOfWork) : base(product)
        {
            _product = product;
            _productPicture = productPicture;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PagedResult<ProductDTO>>> GetProductsPagedAsync(ProductPagingParams pagingParams)
        {
            var query = _product.GetAll()
                .Include(p => p.ProductPictures)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagingParams.SearchTerm))
            {
                var term = pagingParams.SearchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term) ||
                                         (p.Description != null && p.Description.ToLower().Contains(term)));
            }

            if (pagingParams.MinPrice.HasValue)
                query = query.Where(p => p.Price >= pagingParams.MinPrice);

            if (pagingParams.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= pagingParams.MaxPrice);

            query = pagingParams.SortBy switch
            {
                "priceAsc" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderByDescending(p => p.CreateAt) 
            };

            var pagedEntities = await query.ToPagedListAsync(pagingParams.PageIndex, pagingParams.PageSize);

            var productDtos = _mapper.Map<List<ProductDTO>>(pagedEntities.Items);

            var result = new PagedResult<ProductDTO>(
                productDtos,
                pagedEntities.TotalCount,
                pagedEntities.PageIndex,
                pagedEntities.PageSize
            );

            return Result.Ok(result);
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(long Id)
        {
            var product = await _product.GetProductByIdAsync(Id);
            if (product == null)
            {
                return Result.Fail(new NotFoundError($"Product with ID {Id} not found"));
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
                    return Result.Fail(new NotFoundError($"No product found with Id {productDTO.Id}"));
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
                return Result.Fail(new NotFoundError($"No products found with name {name}"));
            }
            return Result.Ok(_mapper.Map<List<ProductDTO>>(products));
        }

        public async Task<Result<List<ProductDTO>>> FilterProductByPriceAsync(decimal priceMin, decimal priceMax)
        {
            var products = await _product.FilterProductByPriceAsync(priceMin, priceMax);
            if (products == null || !products.Any())
            {
                return Result.Fail(new NotFoundError($"No products found in price range {priceMin} to {priceMax}"));
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
                    return Result.Fail(new NotFoundError("No products found"));
                }

                //_product.Delete(product);
                _product.SoftDeleteProduct(product);

                await _unitOfWork.SaveChangesAsync();

                return Result.Ok().WithSuccess("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while deleting the product: " + ex.Message);
            }
        }

        public async Task<Result<ProductPictureDTO>> AddProductPictureAsync(UpdateProductPictureDTO productPictureDTO)
        {
            try
            {
                var productPicture = _mapper.Map<ProductPicture>(productPictureDTO);
                var newProductPiture = await _productPicture.AddProductPictureAsync(productPicture);
                await _unitOfWork.SaveChangesAsync();
                return Result.Ok(_mapper.Map<ProductPictureDTO>(newProductPiture)).WithSuccess("Product picture added successfully");
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while adding the product picture: " + ex.Message);
            }
        }

        public async Task<Result> SetMainProductPictureAsync(long productId, long pictureId)
        {
            try
            {
                var pictures = await _productPicture.GetPicturesByProductIdAsync(productId);
                var newMainPicture = pictures.FirstOrDefault(pp => pp.Id == pictureId);
                if (newMainPicture == null)
                {
                    return Result.Fail(new NotFoundError($"No picture found with ID {pictureId} for product ID {productId}"));
                }
                var currentMainPicture = pictures.FirstOrDefault(pp => pp.IsMain);
                if (currentMainPicture != null)
                {
                    _productPicture.UnsetMainPicture(currentMainPicture);
                }
                _productPicture.SetMainPicture(newMainPicture);
                await _unitOfWork.SaveChangesAsync();
                return Result.Ok().WithSuccess("Main product picture set successfully");
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while setting the main product picture: " + ex.Message);
            }
        }

        public async Task<Result> DeleteProductPictureAsync(long productId, long pictureId)
        {
            try
            {
                var pictures = await _productPicture.GetPicturesByProductIdAsync(productId);
                var pictureToDelete = pictures.FirstOrDefault(pp => pp.Id == pictureId);
                if (pictureToDelete == null)
                {
                    return Result.Fail(new NotFoundError($"No picture found with ID {pictureId} for product ID {productId}"));
                }
                _productPicture.DeleteProductPicture(pictureToDelete);
                await _unitOfWork.SaveChangesAsync();
                return Result.Ok().WithSuccess("Product picture deleted successfully");
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while deleting the product picture: " + ex.Message);
            }
        }

        public async Task<Result<List<ProductPictureDTO>>> GetProductPicturesAsync(long productId)
        {
            try
            {
                var pictures = await _productPicture.GetPicturesByProductIdAsync(productId);
                if (pictures == null || !pictures.Any())
                {
                    return Result.Fail(new NotFoundError($"No pictures found for product ID {productId}"));
                }
                return Result.Ok(_mapper.Map<List<ProductPictureDTO>>(pictures)).WithSuccess("Product pictures retrieved successfully");
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while retrieving the product pictures: " + ex.Message);
            }
        }

        public async Task<Result> UnsetMainProductPictureAsync(long productId, long pictureId)
        {
            try
            {
                var pictures = await _productPicture.GetPicturesByProductIdAsync(productId);
                var pictureToUnset = pictures.FirstOrDefault(pp => pp.Id == pictureId);
                if (pictureToUnset == null)
                {
                    return Result.Fail(new NotFoundError($"No picture found with ID {pictureId} for product ID {productId}"));
                }
                _productPicture.UnsetMainPicture(pictureToUnset);
                await _unitOfWork.SaveChangesAsync();
                return Result.Ok().WithSuccess("Main product picture unset successfully");
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while unsetting the main product picture: " + ex.Message);
            }
        }
    }
}