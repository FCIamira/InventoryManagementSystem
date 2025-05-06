using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryMangmentSystem.Domain;
namespace InventoryMangmentSystem.Domain.Models
{
    public class Transaction_History:BaseModel
    {
            public int Quantity { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }
        [ForeignKey("FromWarehouse")]
            public int? FromWherehosing { get; set; }
            public virtual WhereHosing? FromWarehouse { get; set; }
            [ForeignKey("ToWarehouse")]
            public int? ToWherehosing { get; set; }
            public virtual WhereHosing? ToWarehouse { get; set; }

            [ForeignKey("Product")]
            public int ProductID { get; set; }
            public virtual Product? Product { get; set; }

            [ForeignKey("Transaction_Type")]
            public int Transaction_Type_ID { get; set; }
        public virtual Transaction_type? Transaction_Type { get; set; }
    }
}
