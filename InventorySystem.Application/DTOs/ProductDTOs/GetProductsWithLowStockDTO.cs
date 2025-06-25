using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.DTOs.ProductDTOs
{
    public class GetProductsWithLowStockDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
        public int LowStockThreshold { get; set; }
    }
}
