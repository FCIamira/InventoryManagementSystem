using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Models
{
    public class WhereHosing : BaseModel<Guid>
    {
        public string Name { get; set; }

        public ICollection<WhereHosing_Product> WhereHosing_Products { get; set; }
    }

}
