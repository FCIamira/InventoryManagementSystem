using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Models
{

    public class Transaction_type :BaseModel<int>
    {
        public string Name { get; set; } 

        public ICollection<Transaction_History> TransactionHistories { get; set; }

    }
}
