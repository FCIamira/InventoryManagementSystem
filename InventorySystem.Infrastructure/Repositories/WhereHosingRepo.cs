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
    public class WhereHosingRepo : GenericRepo<WhereHosing, Guid>, IWhereHosing
    {
        private readonly ApplicationContext context;

        public WhereHosingRepo(ApplicationContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
