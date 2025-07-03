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

namespace InventorySystem.Application.Feature.TransactionHistory.Queries
{
    public class StockGetAll: IRequest<Result<IEnumerable<GetAllStock>>>
    {

    }

    public class StockGetAllHandler : IRequestHandler<StockGetAll, Result<IEnumerable<GetAllStock>>>
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

        public async Task<Result<IEnumerable<GetAllStock>>> Handle(StockGetAll request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving all stock transaction histories");

                //var transactions = await _unitOfWork.TransactionHistory.GetAll();
                var transactions = await _unitOfWork.TransactionHistory.GetAllWithDetails();

                if (transactions == null || !transactions.Any())
                {
                    return Result<IEnumerable<GetAllStock>>.Failure(ErrorCode.NotFound, "No stock records found.");
                }

                var dtoList = _mapper.Map<IEnumerable<GetAllStock>>(transactions);

                return Result<IEnumerable<GetAllStock>>.Success(dtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving stock data.");
                return Result<IEnumerable<GetAllStock>>.Failure(ErrorCode.ServerError, "Error occurred while retrieving stock data.");
            }
        }
    }

}
