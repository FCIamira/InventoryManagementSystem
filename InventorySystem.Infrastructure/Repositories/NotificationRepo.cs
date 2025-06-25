using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using InventorySystem.Infrastructure.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Infrastructure.Repositories
{
    public class NotificationRepo : GenericRepo<Notification, Guid>, INotificationRepo
    {
        private readonly ApplicationContext context;

        public NotificationRepo(ApplicationContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
