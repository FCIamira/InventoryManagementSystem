using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.DTOs.Products
{
    public class ProductEditDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Code { get; set; }
        public int Category_Id { get; set; }
    }
}
