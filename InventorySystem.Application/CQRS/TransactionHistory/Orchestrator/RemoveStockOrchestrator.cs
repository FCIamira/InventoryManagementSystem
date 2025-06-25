
using AutoMapper;
using InventorySystem.Application.CQRS.TransactionHistory.Orchestrator.Queries;
using InventorySystem.Application.CQRS.TransactionsHistory.Commands;
using InventorySystem.Application.DTOs.TransactionHistoryDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventorySystem.Application.CQRS.TransactionHistory.Orchestrator
{
    public class RemoveStockOrchestrator : IRequest<Result<string>>
    {
        public RemoveStockDTO StockDto { get; set; }

        public RemoveStockOrchestrator(RemoveStockDTO stockDto)
        {
            StockDto = stockDto;
        }
    }

    public class RemoveStockOrchestratorHandler : IRequestHandler<RemoveStockOrchestrator, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RemoveStockOrchestratorHandler> _logger;

        public RemoveStockOrchestratorHandler(
            IMediator mediator,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            ILogger<RemoveStockOrchestratorHandler> logger)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _logger = logger;
        }

    public async Task<Result<string>> Handle(RemoveStockOrchestrator request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing stock removal for ProductID: {ProductId}, FromWherehosing: {FromWherehosing}, Quantity: {Quantity}",
            request.StockDto.ProductID, request.StockDto.FromWherehosing, request.StockDto.Quantity);

        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogError("HttpContext is null.");
                return Result<string>.Failure(ErrorCode.ServerError, "Request context is unavailable.");
            }

            var user = await _userManager.GetUserAsync(httpContext.User);
            if (user == null)
            {
                _logger.LogError("User not found in claims for stock removal request.");
                return Result<string>.Failure(ErrorCode.Unauthorized, "User is not authenticated.");
            }

            // Validate ProductID
            if (request.StockDto.ProductID == Guid.Empty)
            {
                _logger.LogWarning("ProductID is null in RemoveStockDTO.");
                return Result<string>.Failure(ErrorCode.NotFound, "Product ID is required.");
            }

            // Validate FromWherehosing
            if (!request.StockDto.FromWherehosing.HasValue)
            {
                _logger.LogWarning("FromWherehosing is null in RemoveStockDTO.");
                return Result<string>.Failure(ErrorCode.NotFound, "Source warehouse ID is required.");
            }

            // Retrieve product and warehouse data
            var product = await _unitOfWork.Product.GetById(request.StockDto.ProductID);
            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found.", request.StockDto.ProductID);
                return Result<string>.Failure(ErrorCode.NotFound, "Product not found.");
            }

            var whereHosingFrom = await _unitOfWork.WhereHosing.GetById(request.StockDto.FromWherehosing.Value);
            if (whereHosingFrom == null)
            {
                _logger.LogWarning("Warehouse with ID: {WarehouseId} not found.", request.StockDto.FromWherehosing);
                return Result<string>.Failure(ErrorCode.NotFound, "Source warehouse not found.");
            }

            var whereHosingProducts = await _mediator.Send(new GetWhereHosingProductQuery(
                request.StockDto.ProductID, request.StockDto.FromWherehosing.Value), cancellationToken);
            if (whereHosingProducts == null || whereHosingProducts.Data.Quantity < request.StockDto.Quantity)
            {
                _logger.LogWarning("Insufficient stock for ProductID: {ProductId} in WarehouseID: {WarehouseId}. Available: {AvailableQuantity}, Requested: {RequestedQuantity}",
                    request.StockDto.ProductID, request.StockDto.FromWherehosing, whereHosingProducts?.Data.Quantity ?? 0, request.StockDto.Quantity);
                return Result<string>.Failure(ErrorCode.NotFound, "Insufficient stock in the source warehouse.");
            }

            // Update quantities
            product.Quantity -= request.StockDto.Quantity;
            whereHosingProducts.Data.Quantity -= request.StockDto.Quantity;

            // Create transaction history
            var transactionHistory = new Transaction_History
            {
                UserId = user.Id,
                ProductID = product.Id,
                Quantity = request.StockDto.Quantity,
                Transaction_Type_ID=3,
                CreatedAt = DateTime.UtcNow
            };

            // Update warehouse-product relationship
            var whereHosingProduct = new WhereHosing_Product
            {
                Product_Id = product.Id,
                WhereHosing_Id = whereHosingFrom.Id,
                Quantity = whereHosingProducts.Data.Quantity // Updated quantity
            };

            // Save changes
            _unitOfWork.Product.Update(product.Id,product);
            _unitOfWork.WhereHosing_Product.Update(whereHosingProduct.Id,whereHosingProduct);
            _unitOfWork.TransactionHistory.Add(transactionHistory);

            // Optionally send command to another handler (if needed)
            await _mediator.Send(new RemoveStockCommand(request.StockDto, user.Id), cancellationToken);
             await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully removed stock for ProductID: {ProductId}, Quantity: {Quantity} from WarehouseID: {WarehouseId}",
                request.StockDto.ProductID, request.StockDto.Quantity, request.StockDto.FromWherehosing);

            return Result<string>.Success("Stock removed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while removing stock for ProductID: {ProductId}, FromWherehosing: {FromWherehosing}",
                request.StockDto.ProductID, request.StockDto.FromWherehosing);
            return Result<string>.Failure(ErrorCode.ServerError, "An unexpected error occurred while removing stock.");
        }
    }
}
}
