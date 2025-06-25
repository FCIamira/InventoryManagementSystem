using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


using MediatR;
using System.Collections;
using InventorySystem.Application.DTOs.TransactionHistoryDTOs;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using InventorySystem.Application.Validators;

namespace InventorySystem.Application.CQRS.TransactionsHistory.Commands
{
    public class AddStockCommand :IRequest<Result<string>>
    {
        public AddStockDTO StockDto { get; set; }
        public string UserId { get; set; }

        public AddStockCommand(AddStockDTO StockDto, string userId)
        {
            this.StockDto = StockDto;
            UserId = userId;
        }
    }

    public class AddStockCommandHandler : IRequestHandler<AddStockCommand, Result<string>>
    {
        private readonly IMapper _mapper;
       private IUnitOfWork _unitOfWork;

        public AddStockCommandHandler(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper=mapper;
            _unitOfWork = unitOfWork;
        }   
        public async Task<Result<string>>  Handle(AddStockCommand request, CancellationToken cancellationToken)
        {

            var Stock = _mapper.Map<Transaction_History>(request.StockDto);
            Stock.Transaction_Type_ID = 1;//add
            Stock.UserId = request.UserId;
            await _unitOfWork.TransactionHistory.Add(Stock);

            await _unitOfWork.SaveChangesAsync();
            return Result<string>.Success("Stock added successfully.");

        }
    }
}
