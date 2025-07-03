using AutoMapper;
using InventorySystem.Application.DTOs.TransactionHistoryDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Feature.TransactionsHistory.Commands
{
    public class RemoveStockCommand:IRequest<Result<string>>
    {
        public RemoveStockDTO StockDto { get; set; }
        public string UserId { get; set; }
        public RemoveStockCommand(RemoveStockDTO dto, string userId)
        {
            StockDto = dto;
            UserId = userId;
        }

    }
    public class RemoveStockCommandHandler : IRequestHandler<RemoveStockCommand, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RemoveStockCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<string>> Handle(RemoveStockCommand request, CancellationToken cancellationToken)
        {
         
            var transactionHistory = _mapper.Map<Transaction_History>(request.StockDto);

            transactionHistory.UserId = request.UserId;
            transactionHistory.Transaction_Type_ID = 3;
            await _unitOfWork.TransactionHistory.Add(transactionHistory);

            return Result<string>.Success("Stock removed successfully.");

        }
    }
}
