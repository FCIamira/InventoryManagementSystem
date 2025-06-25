using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Common;
namespace InventorySystem.Domain.Models
{

    public class User : BaseModel<Guid>
    {
        public string FullName { get; set; }
        public string Email { get; set; }

    }

}
