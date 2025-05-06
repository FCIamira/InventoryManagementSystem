using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InventoryMangmentSystem.Domain.DTOs.TransactionHistories;
using InventoryMangmentSystem.Domain.Interfaces;
using InventoryMangmentSystem.Domain.Models;
using InventoryMangmentSystem.DAL.Data;

using MediatR;
using System.Collections;

namespace InventoryMangmentSystem.DAL.CQRS.TransactionsHistory.Commands
{
    public class AddStockCommand :IRequest
    {
        public AddStockDTO StockDto { get; set; }
        public string UserId { get; set; }

        public AddStockCommand(AddStockDTO dto, string userId)
        {
            StockDto = dto;
            UserId = userId;
        }
    }

    public class AddStockCommandHandler : IRequestHandler<AddStockCommand>
    {
        private readonly IMapper _mapper;
       private IGenericRepo<Transaction_History> _StockRepo;

        public AddStockCommandHandler(IMapper mapper,IGenericRepo<Transaction_History> StockRepo)
        {
            _mapper=mapper;
            _StockRepo=StockRepo;
        }   
        public async Task  Handle(AddStockCommand request, CancellationToken cancellationToken)
        {

            var Stock = _mapper.Map<Transaction_History>(request.StockDto);
            Stock.UserId = request.UserId;
            await _StockRepo.Add(Stock);

            await _StockRepo.Save();
        }
    }
}
