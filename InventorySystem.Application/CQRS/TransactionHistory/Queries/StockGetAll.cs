using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Application.Validators;
using InventorySystem.Application.DTOs.TransactionHistoryDTOs;
using InventorySystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using InventorySystem.Domain.Enum;

namespace InventorySystem.Application.CQRS.Stocks.Queries
{
    public class StockGetAll: IRequest<Result<IEnumerable<RemoveStockDTO>>>
    {

    }

    public class StockGetAllHandler : IRequestHandler<StockGetAll, Result<IEnumerable<RemoveStockDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<StockGetAllHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public StockGetAllHandler(IUnitOfWork unitOfWork, IMapper mapper,ILogger<StockGetAllHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
            _logger = logger;
        }

        public async Task<Result<IEnumerable<RemoveStockDTO>>> Handle(StockGetAll request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving all stock transaction histories");

                var transactions = await _unitOfWork.TransactionHistory.GetAll();

                if (transactions == null || !transactions.Any())
                {
                    return Result<IEnumerable<RemoveStockDTO>>.Failure(ErrorCode.NotFound, "No stock records found.");
                }

                var dtoList = _mapper.Map<IEnumerable<RemoveStockDTO>>(transactions);

                return Result<IEnumerable<RemoveStockDTO>>.Success(dtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving stock data.");
                return Result<IEnumerable<RemoveStockDTO>>.Failure(ErrorCode.ServerError, "Error occurred while retrieving stock data.");
            }
        }
    }

}
