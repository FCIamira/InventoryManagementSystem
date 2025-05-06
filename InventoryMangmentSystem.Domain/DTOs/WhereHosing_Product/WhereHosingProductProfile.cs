using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.DTOs.WhereHosing_Product
{
     using InventoryMangmentSystem.Domain.Models;

    public class WhereHosingProductProfile :Profile
    {
        public WhereHosingProductProfile() 
        {
            CreateMap<WhereHosing_Product, WhereHosingProductDTO>().ReverseMap();
        }
    }
}
