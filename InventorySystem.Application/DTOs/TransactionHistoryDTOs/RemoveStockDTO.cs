using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.DTOs.TransactionHistoryDTOs
{
    public class RemoveStockDTO
    {
        public int Quantity { get; set; }

        public Guid? FromWherehosing { get; set; }

        public Guid ProductID { get; set; }

    }
}
