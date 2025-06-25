using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using InventorySystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Infrastructure.Repositories
{
    public class TransactionTypeRepo : GenericRepo<Transaction_type, int>, ITransaction_Type
    {
        private readonly ApplicationContext context;

        public TransactionTypeRepo(ApplicationContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
