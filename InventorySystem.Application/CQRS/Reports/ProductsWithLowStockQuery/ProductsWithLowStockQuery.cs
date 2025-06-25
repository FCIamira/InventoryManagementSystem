using InventorySystem.Application.DTOs.ProductDTOs;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.CQRS.Reports.ProductsWithLowStockQuery
{
    public class ProductsWithLowStockQuery : IRequest<List<GetProductsWithLowStockDTO>>
    {

    }
    public class ProductsWithLowStockQueryHandler : IRequestHandler<ProductsWithLowStockQuery, List<GetProductsWithLowStockDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator mediator;

        public ProductsWithLowStockQueryHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<GetProductsWithLowStockDTO>> Handle(ProductsWithLowStockQuery request, CancellationToken cancellationToken)
        {
            var lowStockProducts = _unitOfWork.Product
    .GetAllWithFilter(p => true)
    .Where(e => !e.IsDeleted)
    .GroupBy(e => new
    {
        e.Id,
        e.Name,
        e.LowStock
    })
    .Select(g => new GetProductsWithLowStockDTO
    {
        ProductId = g.Key.Id,
        ProductName = g.Key.Name,
        TotalQuantity = g.Sum(i => i.Quantity),
        LowStockThreshold = g.Key.LowStock
    })
    .Where(p => p.TotalQuantity < p.LowStockThreshold)
    .ToList();

            return lowStockProducts;
        }
    }
}
