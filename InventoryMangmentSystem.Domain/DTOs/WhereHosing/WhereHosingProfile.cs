using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryMangmentSystem.Domain.Models;
namespace InventoryMangmentSystem.Domain.DTOs.WhereHosing
{
    public class WhereHosingProfile :Profile
    {
        public WhereHosingProfile()
        {
            CreateMap<WhereHosingDTO, InventoryMangmentSystem.Domain.Models.WhereHosing>().ReverseMap();

        }
    }
}
