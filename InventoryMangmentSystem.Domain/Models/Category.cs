using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.Models
{
    public class Category:BaseModel
    {
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
