using AutoMapper;
using InventorySystem.Application.DTOs.TransactionHistoryDTOs;
using InventorySystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Helper.Mapper
{
    public class TransationHistoryProfile : Profile
    {
        public TransationHistoryProfile()
        {
            CreateMap<TransfarStockDTO, Transaction_History>()
            .ForMember(dest => dest.Transaction_Type, opt => opt.Ignore()).ReverseMap();

            CreateMap<AddStockDTO, Transaction_History>()
    .ForMember(dest => dest.Transaction_Type, opt => opt.Ignore());


            CreateMap<RemoveStockDTO, Transaction_History>()
                 .ForMember(dest => dest.Transaction_Type, opt => opt.Ignore()).ReverseMap();

            CreateMap<Transaction_History, GetAllStock>()
              .ForMember(dest => dest.ProductName,
                  opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
              .ForMember(dest => dest.FromWherehosing,
                  opt => opt.MapFrom(src => src.FromWarehouse != null ? src.FromWarehouse.Name : null))
              .ForMember(dest => dest.ToWherehosing,
                  opt => opt.MapFrom(src => src.ToWarehouse != null ? src.ToWarehouse.Name : null))
              .ForMember(dest => dest.Transaction_Type_Name,
                  opt => opt.MapFrom(src => src.Transaction_Type != null ? src.Transaction_Type.Name : null)).ReverseMap();



        }
    }
}
