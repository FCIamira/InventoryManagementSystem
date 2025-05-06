using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.DTOs.TransactionHistories
{
    public class TransfarStockDTO
    {
        public int Id {  get; set; }
        public int Quantity { get; set; }

        public int? FromWherehosing { get; set; }

        public int? WhereHosing_Id { get; set; }

        public int ProductID { get; set; }

        public int Transaction_Type_ID { get; set; } 
    }
}
