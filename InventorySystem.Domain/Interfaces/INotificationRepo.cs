using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Models;

namespace InventorySystem.Domain.Interfaces
{
    public interface INotificationRepo : IGenericRepo<Notification,Guid>
    {
    }
}
