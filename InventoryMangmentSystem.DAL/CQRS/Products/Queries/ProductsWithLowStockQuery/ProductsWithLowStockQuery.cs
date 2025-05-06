using InventoryMangmentSystem.Domain.DTOs.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Products.Queries.ProductsWithLowStockQuery
{
    public class ProductsWithLowStockQuery : IRequest<List<GetProductsWithLowStockDTO>>
    {

    }
    public class ProductsWithLowStockQueryHandler : IRequestHandler<ProductsWithLowStockQuery, List<GetProductsWithLowStockDTO>>
    {
        private readonly IGenericRepo<Transaction_History> genericRepository;
        private readonly IMediator mediator;

        public ProductsWithLowStockQueryHandler(IMediator mediator, IGenericRepo<Transaction_History> genericRepository)
        {
            this.mediator = mediator;
            this.genericRepository = genericRepository;
        }
        public async Task<List<GetProductsWithLowStockDTO>> Handle(ProductsWithLowStockQuery request, CancellationToken cancellationToken)
        {
            var lowStockProducts = genericRepository
    .Get(p => true)
    .Where(e => !e.IsDelete)
    .GroupBy(e => new
    {
        e.ProductID,
        e.Product.Name,
        e.Product.LowStock
    })
    .Select(g => new GetProductsWithLowStockDTO
    {
        ProductId = g.Key.ProductID,
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
