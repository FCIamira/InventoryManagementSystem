using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        public string? Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int LowStock { get; set; }

        public int Quantity { get; set; }
        public string Code { get; set; }
        public Guid CategoryId { get; set; }
        
    }
}
