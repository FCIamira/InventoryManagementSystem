using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Models;
using InventorySystem.Application.DTOs.WhereHosingDTOs;

namespace InventorySystem.Application.Helper.Mapper
{
    public class WhereHosingProfile : Profile
    {
        public WhereHosingProfile()
        {
            CreateMap<AddWhereHosingDTO,WhereHosing>().ReverseMap();
            CreateMap<GetAllWhereHosingDTO, WhereHosing>().ReverseMap();


        }
    }
}
