using AutoMapper;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.DTOs;
namespace VTT_SHOP_CORE.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, ProductDTO>();   
            CreateMap<Product, CreateProductDTO>();
            CreateMap<Product, UpdateProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserDTO, User>();
            CreateMap<User,UserDTO>();
            CreateMap<User,UserCreateDTO>();
            CreateMap<UserRoleDTO,Role>();
            CreateMap<Role,UserRoleDTO>();
            CreateMap<CartItemCreateDTO, CartItem>();
            CreateMap<CartItem, CartItemCreateDTO>();
            CreateMap<CartItemDTO, CartItem>();
            CreateMap<CartItem, CartItemDTO>();
        }
    }
}
