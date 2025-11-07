using AutoMapper;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.DTOs;
namespace VTT_SHOP_CORE.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductPictureId,opt => opt.MapFrom(src => src.ProductPictures.FirstOrDefault(pp => pp.IsMain).Id))
                .ForMember(dest => dest.ProductPicture, opt => opt.MapFrom(src => src.ProductPictures.FirstOrDefault(pp => pp.IsMain).PictureUrl));   
            CreateMap<Product, CreateProductDTO>();
            CreateMap<Product, UpdateProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();
            CreateMap<ProductPicture, UpdateProductPictureDTO>();
            CreateMap<UpdateProductPictureDTO, ProductPicture>();
            CreateMap<ProductPictureDTO, ProductPicture>();
            CreateMap<ProductPicture, ProductPictureDTO>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserDTO, User>();
            CreateMap<User,UserDTO>();
            CreateMap<User,UserCreateDTO>();
            CreateMap<UserRoleDTO,Role>();
            CreateMap<Role,UserRoleDTO>();
            CreateMap<CartItemCreateDTO, CartItem>();
            CreateMap<CartItem, CartItemCreateDTO>();
            CreateMap<CartItemDTO, CartItem>();
            CreateMap<CartItem, CartItemDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.ProductPicture, opt => opt.MapFrom(src =>
                src.Product.ProductPictures.FirstOrDefault(pp => pp.IsMain).PictureUrl));
            CreateMap<WishListItem, WishListDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Product.Quantity))
            .ForMember(dest => dest.ProductPicture, opt => opt.MapFrom(src =>
                src.Product.ProductPictures.FirstOrDefault(pp => pp.IsMain).PictureUrl));
        }
    }
}
