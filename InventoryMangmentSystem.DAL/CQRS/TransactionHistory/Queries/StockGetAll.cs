using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryMangmentSystem.Domain.DTOs.TransactionHistories;
using InventoryMangmentSystem.DAL.Data;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Stocks.Queries
{
    public class StockGetAll: IRequest<IEnumerable<RemoveStockDTO>>
    {

    }

    public class StockGetAllHandler : IRequestHandler<StockGetAll, IEnumerable<RemoveStockDTO>> // Return IEnumerable
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepo<Transaction_History> _StockRepo;

        public StockGetAllHandler(IGenericRepo<Transaction_History>  StockRepo, IMapper mapper)
        {
            _StockRepo = StockRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RemoveStockDTO>> Handle(StockGetAll request, CancellationToken cancellationToken)
        {
            return await _StockRepo.GetAll()
                                      .ProjectTo<RemoveStockDTO>(_mapper.ConfigurationProvider)
                                      .ToListAsync();
        }
    }

}
