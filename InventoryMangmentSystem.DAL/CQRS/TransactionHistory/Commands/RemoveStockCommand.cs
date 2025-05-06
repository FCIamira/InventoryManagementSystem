using AutoMapper;
using InventoryMangmentSystem.Domain.DTOs.TransactionHistories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.TransactionsHistory.Commands
{
    public class RemoveStockCommand:IRequest
    {
        public RemoveStockDTO StockDto { get; set; }
        public string UserId { get; set; }
        public RemoveStockCommand(RemoveStockDTO dto, string userId)
        {
            StockDto = dto;
            UserId = userId;
        }

    }
    public class RemoveStockCommandHandler : IRequestHandler<RemoveStockCommand>
    {
        private readonly IGenericRepo<Transaction_History> _StockRepo;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RemoveStockCommandHandler(IMediator mediator, IGenericRepo<Transaction_History> StockRepo, IMapper mapper)
        {
            _StockRepo = StockRepo;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Handle(RemoveStockCommand request, CancellationToken cancellationToken)
        {
         
            var transactionHistory = _mapper.Map<Transaction_History>(request.StockDto);

            transactionHistory.UserId = request.UserId;

            await _StockRepo.Add(transactionHistory);

            await _StockRepo.Save();

        }
    }
}
