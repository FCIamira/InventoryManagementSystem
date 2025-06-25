using InventorySystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.DTOs.TransactionHistoryDTOs
{
    public class GetAllStock
    {
        public int Quantity { get; set; }

        public string FromWherehosing { get; set; }
        public string ToWherehosing { get; set; }

        public string ProductName { get; set; }

        public string Transaction_Type_Name { get; set; }
    }

}
