using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Domain.Interfaces
{
    public interface ICategoryRepo:IGenericRepo<Category,Guid>
    {
    }
}
