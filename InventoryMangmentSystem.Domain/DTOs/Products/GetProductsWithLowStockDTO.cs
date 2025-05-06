using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.DTOs.Products
{
    public class GetProductsWithLowStockDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
        public int LowStockThreshold { get; set; }
    }
}
