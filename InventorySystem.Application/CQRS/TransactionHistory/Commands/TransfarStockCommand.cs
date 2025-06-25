using AutoMapper;
using InventorySystem.Application.DTOs.TransactionHistoryDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.CQRS.TransactionsHistory.Commands
{
    public class TransfarStockCommand : IRequest<Result<string>>
    {
        public TransfarStockDTO StockDto { get; set; }
        public TransfarStockCommand(TransfarStockDTO Stock)
        {
            StockDto = Stock;
        }

    }
    public class TransfarStockCommandHandler : IRequestHandler<TransfarStockCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransfarStockCommandHandler(
            IUnitOfWork UnitOfWork,
            IMapper mapper, IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = UnitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(TransfarStockCommand request, CancellationToken cancellationToken)
        {
            var dto = request.StockDto;
            var httpContext = _httpContextAccessor.HttpContext;
            var user = await _userManager.GetUserAsync(httpContext.User);

            if (user == null)
            {
                return Result<string>.Failure(ErrorCode.BadRequest, "User not found.");
            }

            if (dto.Quantity <= 0)
                return Result<string>.Failure("Invalid quantity.");

            var fromStock = await _unitOfWork.WhereHosing_Product
                .GetAllWithFilter(x => x.Product_Id == dto.ProductID && x.WhereHosing_Id == dto.FromWherehosing)
                .FirstOrDefaultAsync();

            if (fromStock == null || fromStock.Quantity < dto.Quantity)
                return Result<string>.Failure("Insufficient stock in source warehouse.");

            var toStock = await _unitOfWork.WhereHosing_Product
                .GetAllWithFilter(x => x.Product_Id == dto.ProductID && x.WhereHosing_Id == dto.WhereHosing_Id)
                .FirstOrDefaultAsync();

            if (toStock == null)
            {
                toStock = new WhereHosing_Product
                {
                    Product_Id = dto.ProductID,
                    WhereHosing_Id = dto.WhereHosing_Id,
                    Quantity = dto.Quantity
                };
                await _unitOfWork.WhereHosing_Product.Add(toStock);
            }
            else
            {
                toStock.Quantity += dto.Quantity;
                await _unitOfWork.WhereHosing_Product.Update(toStock.Id, toStock);
            }

            fromStock.Quantity -= dto.Quantity;
            await _unitOfWork.WhereHosing_Product.Update(fromStock.Id, fromStock);

            var transaction = new Transaction_History
            {
                ProductID = dto.ProductID,
                FromWherehosing = dto.FromWherehosing,
                ToWherehosing = dto.WhereHosing_Id,
                Quantity = dto.Quantity,
                Transaction_Type_ID = 2,
                UserId=user.Id,
            };

            await _unitOfWork.TransactionHistory.Add(transaction);
            await _unitOfWork.SaveChangesAsync();

            return Result<string>.Success("Stock transferred successfully.");

        }

    }
}
