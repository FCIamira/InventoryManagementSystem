using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.TransactionHistory.Orchestrator.Queries
{
    public class GetWhereHosingProductQuery : IRequest<WhereHosing_Product>
    {
        public int ProductId { get; set; }
        public int WhereHosingId { get; set; }

        public GetWhereHosingProductQuery(int productId, int whereHosingId)
        {
            ProductId = productId;
            WhereHosingId = whereHosingId;
        }

    }
    public class GetWhereHosingProductQueryHandler : IRequestHandler<GetWhereHosingProductQuery, WhereHosing_Product>
    {
        private readonly IGenericRepo<WhereHosing_Product> _productRepo;

        public GetWhereHosingProductQueryHandler(IGenericRepo<WhereHosing_Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<WhereHosing_Product> Handle(GetWhereHosingProductQuery request, CancellationToken cancellationToken)
        {
            var whereHosingProduct = _productRepo.GetAll()
        .FirstOrDefault(wp => wp.Product_Id == request.ProductId && wp.WhereHosing_Id == request.WhereHosingId);
            if (whereHosingProduct == null )
            {
                whereHosingProduct = new WhereHosing_Product
                {
                    Product_Id = request.ProductId,
                    WhereHosing_Id = request.WhereHosingId,
                    Quantity = 0 
                };

                await _productRepo.Add(whereHosingProduct);
                await _productRepo.Save();
            }
            return whereHosingProduct ?? throw new Exception("WhereHosing_Products not found.");
        }
    }


}
