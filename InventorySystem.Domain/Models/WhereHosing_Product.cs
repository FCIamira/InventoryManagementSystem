using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Models
{
    public class WhereHosing_Product : BaseModel<Guid>
    {
        [ForeignKey("WhereHosing")]
        public Guid? WhereHosing_Id { get; set; }
        public virtual WhereHosing WhereHosing { get; set; }
        [ForeignKey("Product")]
        public Guid Product_Id { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }

}
