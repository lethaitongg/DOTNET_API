using AutoMapper;
using DEMO_BUOI07_API.DTO;
using DEMO_BUOI07_API.Models;

namespace DEMO_BUOI07_API.Helpers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductPost, Product>();
            CreateMap<Product, ProductGet>()
                .ForMember(
                    dest => dest.ImageNames, 
                    opt => opt.MapFrom(src => src.ProductImages.Select(productImage => productImage.Name))
                ).ReverseMap();
        }
    }
}
