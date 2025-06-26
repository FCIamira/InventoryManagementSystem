using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using InventorySystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Infrastructure.Repositories
{


    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        private IProductRepo _productRepository;
        private ICategoryRepo _CategoryRepository;

        private ITransaction_History _TransactionHistoryRepository;
        private ITransaction_Type _TransactionTypeRepository;
        private IWhereHosing _WhereHosingRepository;
        private INotificationRepo _notificationRepository;
        private IWhereHosing_Product _WhereHosingProductRepository;
        private IStockCheckerService _StockChecker;
        public UnitOfWork(ApplicationContext  applicationDBContext)
        {
            _context = applicationDBContext;
        }

        #region Product
        public IProductRepo Product
        {
            get
            {
                if (_productRepository is null)
                {
                    _productRepository = new ProductRepo(_context);
                }
                return _productRepository;
            }
        }
        #endregion

        #region Category
        public ICategoryRepo Category
        {
            get
            {
                if (_CategoryRepository is null)
                {
                    _CategoryRepository = new CategoryRepo(_context);
                }
                return _CategoryRepository;
            }
        }
        #endregion

        #region ITransaction_History
        public ITransaction_History TransactionHistory
        {
            get
            {
                if (_TransactionHistoryRepository is null)
                {
                    _TransactionHistoryRepository = new TransactionHistoryRepo(_context);
                }
                return _TransactionHistoryRepository;
            }
        }
        #endregion


        #region ITransaction_Type
        public ITransaction_Type TransactionType
        {
            get
            {
                if (_TransactionTypeRepository is null)
                {
                    _TransactionTypeRepository = new TransactionTypeRepo(_context);
                }
                return _TransactionTypeRepository;
            }
        }
        #endregion

        #region WhereHosing
        public IWhereHosing WhereHosing
        {
            get
            {
                if (_WhereHosingRepository is null)
                {
                    _WhereHosingRepository = new WhereHosingRepo(_context);
                }
                return _WhereHosingRepository;
            }
        }
        #endregion

        #region WhereHosing_Product
        public IWhereHosing_Product WhereHosing_Product
        {
            get
            {
                if (_WhereHosingProductRepository is null)
                {
                    _WhereHosingProductRepository = new WhereHosingProductRepo(_context);
                }
                return _WhereHosingProductRepository;
            }
        }
        #endregion


        #region Notification
        public INotificationRepo Notification
        {
            get
            {
                if (_notificationRepository is null)
                {
                    _notificationRepository = new NotificationRepo(_context);
                }
                return _notificationRepository;
            }
        }
        #endregion


        #region StockChecker
        public IStockCheckerService StockChecker
        {
            get
            {
                if (_StockChecker is null)
                {
                    _StockChecker = new StockCheckRepo(_context);
                }
                return _StockChecker;
            }
        }
        #endregion

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}