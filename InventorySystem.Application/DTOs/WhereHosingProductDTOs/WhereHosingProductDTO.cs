using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.DTOs.WhereHosingProductDTOs
{
    public  class WhereHosingProductDTO
    {
        public int Product_Id { get; set; }
        public int WhereHosing_Id { get; set; }
        public int Quantity { get; set; }
    }
}
