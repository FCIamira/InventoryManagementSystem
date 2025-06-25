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
    public class ProductRepo : GenericRepo<Product, Guid>, IProductRepo
    {
        private readonly ApplicationContext context;

        public ProductRepo(ApplicationContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(Guid categoryId)
        {
            return await Task.FromResult(context.Products.Where(p => p.Category_Id == categoryId && !p.IsDeleted).AsEnumerable());

        }
    }
}
