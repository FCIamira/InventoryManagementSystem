using AutoMapper;
using InventoryMangmentSystem.Domain.DTOs.TransactionHistories;
using InventoryMangmentSystem.Domain.Interfaces;
using InventoryMangmentSystem.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.TransactionsHistory.Commands
{
    public class TransfarStockCommand:IRequest
    {
        public TransfarStockDTO StockDto { get; set; }
        public TransfarStockCommand(TransfarStockDTO Stock)
        {
            StockDto  =Stock;
        }

    }
    public class TransfarStockCommandHandler : IRequestHandler<TransfarStockCommand>
    {
        private readonly IGenericRepo<WhereHosing_Product> _warehouseRepo;
        private readonly IGenericRepo<Transaction_History> _transactionRepo;
        private readonly IMapper _mapper;
        public TransfarStockCommandHandler(
            IGenericRepo<WhereHosing_Product> warehouseRepo,
            IGenericRepo<Transaction_History> transactionRepo,
            IMapper mapper)
        {
            _warehouseRepo = warehouseRepo;
            _transactionRepo = transactionRepo;
            _mapper = mapper;
        }

        public async Task Handle(TransfarStockCommand request, CancellationToken cancellationToken)
        {
            var dto = request.StockDto;

            var fromStock = await _warehouseRepo
     .Get(x => x.Product_Id == dto.ProductID && x.WhereHosing_Id == dto.FromWherehosing)
     .FirstOrDefaultAsync();

            

            if (fromStock == null || fromStock.Quantity < dto.Quantity)
                throw new InvalidOperationException("Insufficient stock in source warehouse.");

            var toStock = await _warehouseRepo
                .Get(x => x.Product_Id == dto.ProductID && x.WhereHosing_Id == dto.WhereHosing_Id)
                .FirstOrDefaultAsync();

            if (toStock == null)
            {
                toStock = new WhereHosing_Product
                {
                    Product_Id = dto.ProductID,
                    WhereHosing_Id = dto.WhereHosing_Id,
                    Quantity = 0
                };
                await _warehouseRepo.Add(toStock); 
            }


            fromStock.Quantity -= dto.Quantity;
            toStock.Quantity += dto.Quantity;

            await _warehouseRepo.Update(fromStock);
            await _warehouseRepo.Update(toStock);

            await _warehouseRepo.Save();

            var transaction = new Transaction_History
            {
                ProductID = dto.ProductID,
                FromWherehosing = dto.FromWherehosing,
                ToWherehosing = dto.WhereHosing_Id,
                Quantity = dto.Quantity,
                Transaction_Type_ID = 4,
            };

            await _transactionRepo.Add(transaction);
            await _transactionRepo.Save();
        }
    }

}
