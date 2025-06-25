using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InventorySystem.Application.DTOs.TransactionHistoryDTOs;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Models;
using InventorySystem.Domain.Enum;
using InventorySystem.Application.CQRS.TransactionHistory.Orchestrator.Queries;
using InventorySystem.Application.CQRS.TransactionsHistory.Commands;
using InventorySystem.Application.CQRS.Account.Commands;
using InventorySystem.Application.CQRS.Account.Orchestrator;



namespace InventorySystem.Application.CQRS.TransactionHistory.Orchestrator
{

    public class AddStockOrchestrator :IRequest<Result<string>>
    {
        public AddStockDTO StockDto { get; set; }
         

        public AddStockOrchestrator(AddStockDTO stockDto)
        {
            StockDto = stockDto;
        }
    }
    public class AddStockOrchestratorHandler : IRequestHandler<AddStockOrchestrator, Result<string>>
    {
        private IMediator _mediator;

        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;


        public AddStockOrchestratorHandler(IMediator mediator, 
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<Result<string>> Handle(AddStockOrchestrator request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = await _userManager.GetUserAsync(httpContext.User);

            if (user == null)
            {
                return Result<string>.Failure(ErrorCode.BadRequest, "User not found.");
            }

            var product = await _unitOfWork.Product.GetById(request.StockDto.ProductID);
            if (product == null)
            {
                return Result<string>.Failure(ErrorCode.NotFound, "Product not found.");
            }

            if (!request.StockDto.FromWherehosing.HasValue)
            {
                return Result<string>.Failure(ErrorCode.BadRequest, "FromWherehosing is required.");
            }

            //var whereHosingFrom = await _unitOfWork.WhereHosing.GetById(request.StockDto.FromWherehosing.Value);

            var whereHosingProductResult = await _mediator.Send(new GetWhereHosingProductQuery(
                request.StockDto.ProductID,
                request.StockDto.FromWherehosing.Value));

            //var TransactionHistory = await _mediator.Send(new GetWhereHosingProductQuery(request.StockDto.ProductID,));
            if (whereHosingProductResult == null || !whereHosingProductResult.IsSuccess || whereHosingProductResult.Data == null)
            {
                return Result<string>.Failure(ErrorCode.NotFound, "WhereHosing_Product record not found.");
            }

            var whereHosingProduct = whereHosingProductResult.Data;

            // Update quantities
            product.Quantity += request.StockDto.Quantity;
            whereHosingProduct.Quantity += request.StockDto.Quantity;
            
            // Save updates
            _unitOfWork.Product.Update(product.Id, product);
            _unitOfWork.WhereHosing_Product.Update(whereHosingProduct.Id, whereHosingProduct);

            await _unitOfWork.SaveChangesAsync();

            // Add transaction
            var AddResult = await _mediator.Send(new AddStockCommand(request.StockDto, user.Id));
            if (!AddResult.IsSuccess)
                return Result<string>.Failure(AddResult.Errorcode, AddResult.Message);

            return Result<string>.Success("Stock added successfully.");
        }

    }

}

