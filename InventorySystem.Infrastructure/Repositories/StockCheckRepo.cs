
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Infrastructure.Repositories
{
    public class StockCheckRepo :IStockCheckerService
    {
        private readonly ApplicationContext context;

        public StockCheckRepo(ApplicationContext _context) 
        {
            context = _context;
        }

        public Task CheckLowStockAsync()
        {
            throw new NotImplementedException();
        }
    }
}