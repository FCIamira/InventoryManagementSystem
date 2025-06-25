using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.DTOs.TransactionHistoryDTOs
{
    public class TransactionHistoryDTO
    {
        public int? productName { get; set; }
        public string? CategoryName { get; set; }
        public string TransactionType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
