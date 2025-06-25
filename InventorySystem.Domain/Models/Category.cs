using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Models
{
    public class Category : BaseModel<Guid>
    {
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
