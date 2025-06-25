using InventorySystem.Application.Validators;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.CQRS.TransactionHistory.Orchestrator.Queries
{
    public class GetWhereHosingProductQuery : IRequest<Result<WhereHosing_Product>>
    {
        public Guid ProductId { get; set; }
        public Guid WhereHosingId { get; set; }

        public GetWhereHosingProductQuery(Guid productId, Guid whereHosingId)
        {
            ProductId = productId;
            WhereHosingId = whereHosingId;
        }

    }
    public class GetWhereHosingProductQueryHandler : IRequestHandler<GetWhereHosingProductQuery, Result<WhereHosing_Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWhereHosingProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async  Task<Result<WhereHosing_Product>> Handle(GetWhereHosingProductQuery request, CancellationToken cancellationToken)
        {
            var allItems = await _unitOfWork.WhereHosing_Product.GetAll();
            var whereHosingProduct = allItems
                .FirstOrDefault(wp => wp.Product_Id == request.ProductId && wp.WhereHosing_Id == request.WhereHosingId);

            if (whereHosingProduct == null)
            {
                whereHosingProduct = new WhereHosing_Product
                {
                    Product_Id = request.ProductId,
                    WhereHosing_Id = request.WhereHosingId,
                    Quantity = 0
                };

                await _unitOfWork.WhereHosing_Product.Add(whereHosingProduct);
                await _unitOfWork.SaveChangesAsync();
            }

            return Result<WhereHosing_Product>.Success(whereHosingProduct);
        }
    }


}
