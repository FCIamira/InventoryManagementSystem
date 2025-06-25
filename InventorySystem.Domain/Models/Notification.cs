using InventorySystem.Domain.Common;
using InventorySystem.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Domain.Models
{
    public class Notification : BaseModel<Guid>
    {
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;

        public NotificationType Type { get; set; }
    }


}

