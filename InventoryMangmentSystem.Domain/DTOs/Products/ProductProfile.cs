using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace InventoryMangmentSystem.Domain.DTOs.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>().ForMember(dest=>dest.CategoryName,
                opit=>opit.MapFrom(src=>src.Category.Name))
                .ReverseMap();

            CreateMap<Product, ProductDetailsDTO>().ForMember(dest => dest.CategoryName,
                opit => opit.MapFrom(src => src.Category.Name))
               .ReverseMap();

            CreateMap<ProductEditDTO, Product>().ForMember(dest => dest.Category_Id,
                opit => opit.MapFrom(src => src.Category_Id)).ReverseMap();

           CreateMap<ProductCreateDTO, Product>().ForMember(dest => dest.Category_Id,
                opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Category, opt => opt.Ignore()) ;
          CreateMap<Product, ProductEditDTO>().ForMember(dest => dest.Category_Id,
      opt => opt.MapFrom(src => src.Category_Id)).ReverseMap();



        }
    }
}
