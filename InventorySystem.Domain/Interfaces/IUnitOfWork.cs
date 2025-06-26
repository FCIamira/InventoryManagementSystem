using InventorySystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Domain.Interfaces
{


    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepo Category { get; }
        IProductRepo Product { get; }
        ITransaction_History TransactionHistory { get; }
        ITransaction_Type TransactionType { get; }
        IWhereHosing WhereHosing { get; }
        IWhereHosing_Product WhereHosing_Product { get; }
        INotificationRepo Notification { get; }
        IStockCheckerService StockChecker {  get; }
        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
