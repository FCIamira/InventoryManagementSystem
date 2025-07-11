﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
namespace InventorySystem.Domain.Interfaces
{
    public interface ITransaction_History:IGenericRepo<Transaction_History,Guid>
    {
        public  Task<IEnumerable<Transaction_History>> GetAllWithDetails();

    }
}
