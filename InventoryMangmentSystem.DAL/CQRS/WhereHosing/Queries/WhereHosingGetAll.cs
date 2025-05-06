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
using InventoryMangmentSystem.Domain.DTOs.WhereHosing;

namespace InventoryMangmentSystem.DAL.CQRS.WhereHosings.Queries
{
    public class WhereHosingGetAll: IRequest<IEnumerable<WhereHosingDTO>>
    {

    }

    public class WhereHosingGetAllHandler : IRequestHandler<WhereHosingGetAll, IEnumerable<WhereHosingDTO>> // Return IEnumerable
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepo<WhereHosing> _WhereHosingRepo;

        public WhereHosingGetAllHandler(IGenericRepo<WhereHosing>  WhereHosingRepo, IMapper mapper)
        {
            _WhereHosingRepo = WhereHosingRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WhereHosingDTO>> Handle(WhereHosingGetAll request, CancellationToken cancellationToken)
        {
            return await _WhereHosingRepo.GetAll()
                                      .ProjectTo<WhereHosingDTO>(_mapper.ConfigurationProvider)
                                      .ToListAsync();
        }
    }

}
