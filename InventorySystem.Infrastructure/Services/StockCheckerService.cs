using InventorySystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Infrastructure.Services
{
   
        public class StockCheckerService : IStockCheckerService
        {
            private readonly IUnitOfWork _unitOfWork;

            public StockCheckerService(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task CheckLowStockAsync()
            {
                var products = await _unitOfWork.Product.GetAll();
                var lowStock = products.Where(p => p.Quantity < 5).ToList();

                foreach (var product in lowStock)
                {
                    Console.WriteLine($"[LOW STOCK] {product.Name} has only {product.Quantity} items left.");
                }
            }
        }
    }


