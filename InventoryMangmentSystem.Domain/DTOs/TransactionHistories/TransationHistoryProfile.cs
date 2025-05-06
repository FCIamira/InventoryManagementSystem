using AutoMapper;
using InventoryMangmentSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.DTOs.TransactionHistories
{
    public class TransationHistoryProfile: Profile
    {
        public TransationHistoryProfile()
        {
            CreateMap<TransfarStockDTO, Transaction_History>()
            .ForMember(dest => dest.Transaction_Type, opt => opt.Ignore()).ReverseMap();

            CreateMap<AddStockDTO, Transaction_History>()
    .ForMember(dest => dest.Transaction_Type, opt => opt.Ignore());

               
            CreateMap<RemoveStockDTO ,Transaction_History>()
                 .ForMember(dest => dest.Transaction_Type, opt => opt.Ignore()).ReverseMap();



        }
    }
}
