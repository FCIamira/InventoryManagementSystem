using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InventorySystem.Domain.Models
{
    //ApplicationUser
    public class ApplicationUser: IdentityUser
    {
        public String FirstName { get; set; } = null!;
        public String LastName { get; set; } = null!;


        public IEnumerable<Transaction_History>? TransferTransactions { get; set; }
    }
}
