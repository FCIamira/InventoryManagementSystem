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
    public class WhereHosingProductRepo : GenericRepo<WhereHosing_Product, Guid>,IWhereHosing_Product
    {
        private readonly ApplicationContext context;

        public WhereHosingProductRepo(ApplicationContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
