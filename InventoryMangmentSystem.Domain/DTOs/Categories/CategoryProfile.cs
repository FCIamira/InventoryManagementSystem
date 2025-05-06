using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.DTOs.Categories
{
    public class CategoryProfile :Profile
    {
     public CategoryProfile() 
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CategoryCreateDTO,Category>();
            CreateMap<CategoryEditDTO,Category>();
        }
    }
}
