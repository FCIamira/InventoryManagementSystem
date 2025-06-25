using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.DTOs.TransactionHistoryDTOs
{
    public class TransfarStockDTO
    {
        public Guid Id {  get; set; }
        public int Quantity { get; set; }

        public Guid? FromWherehosing { get; set; }

        public Guid? WhereHosing_Id { get; set; }

        public Guid ProductID { get; set; }

    }
}
