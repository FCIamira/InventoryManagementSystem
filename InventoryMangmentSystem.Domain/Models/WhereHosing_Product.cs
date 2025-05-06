using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.Models
{
    public class WhereHosing_Product:BaseModel
    {
        [ForeignKey("WhereHosing")]
        public int? WhereHosing_Id { get; set; }
        public virtual WhereHosing WhereHosing { get; set; }
        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }

}
