using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.Models
{
    
    public class Transaction_type:BaseModel
    {
         public string Name { get; set; } // : "Add-->2", "Remove--->3", "Transfer----4>"

        public ICollection<Transaction_History> TransactionHistories { get; set; }
    
}
}
