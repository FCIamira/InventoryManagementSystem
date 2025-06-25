using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Models
{
    public class Product : BaseModel<Guid>
    {

        public string? Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int LowStock { get; set; }
        public string Code { get; set; }
        [ForeignKey("Category")]
        public Guid Category_Id { get; set; }
        public virtual Category? Category { get; set; }

        public ICollection<Transaction_History>? TransactionHistories { get; set; }
        public ICollection<WhereHosing_Product>? WhereHosing_Products { get; set; }
    }

}

