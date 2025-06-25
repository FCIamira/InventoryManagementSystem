using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using InventorySystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories
{
    public class TransactionHistoryRepo : GenericRepo<Transaction_History, Guid>, ITransaction_History
    {
        private readonly ApplicationContext context;

        public TransactionHistoryRepo(ApplicationContext _context) : base(_context)
        {
            context = _context;
        }
        public async Task<IEnumerable<Transaction_History>> GetAllWithDetails()
        {
            return await context.TransactionHistories
                .Include(x => x.Product)
                .Include(x => x.FromWarehouse)
                .Include(x => x.ToWarehouse)
                .Include(x => x.Transaction_Type)
                .ToListAsync();
        }


    }
}
