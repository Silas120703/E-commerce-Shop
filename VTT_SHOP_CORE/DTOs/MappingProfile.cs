using AutoMapper;
using VTT_SHOP_DATABASE.Entities;
namespace VTT_SHOP_CORE.DTOs
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
        }
    }
}
