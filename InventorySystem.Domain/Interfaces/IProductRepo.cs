using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Domain.Interfaces
{
    public interface IProductRepo : IGenericRepo<Product,Guid>
    {
        public Task<IEnumerable<Product>> GetProductByCategory(Guid categoryId);


    }
}
