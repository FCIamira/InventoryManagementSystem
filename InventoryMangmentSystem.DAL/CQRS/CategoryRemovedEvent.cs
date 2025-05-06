using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace InventoryMangmentSystem.DAL.CQRS
{
    internal class CategoryRemovedEvent : INotification
    {
        public int Id { get; set; }

    }
}
