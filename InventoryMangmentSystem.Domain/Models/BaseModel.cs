using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.Domain.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
