using AutoMapper;
using InventorySystem.Application.DTOs.WhereHosingProductDTOs;
using InventorySystem.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Helper.Mapper
{
   

    public class WhereHosingProductProfile : Profile
    {
        public WhereHosingProductProfile()
        {
            CreateMap<WhereHosing_Product, WhereHosingProductDTO>().ReverseMap();
        }
    }
}
