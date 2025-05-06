using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.WhereHosings.Commands
{
    public class RemoveWhereHosingCommand:IRequest
    {
        public int Id { get; set; }
       
    }
    public class RemoveWhereHosingCommandHandler : IRequestHandler<RemoveWhereHosingCommand>
    {
        private readonly IGenericRepo<WhereHosing> _WhereHosingRepo;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RemoveWhereHosingCommandHandler(IMediator mediator, IGenericRepo<WhereHosing> WhereHosingRepo, IMapper mapper)
        {
            _WhereHosingRepo = WhereHosingRepo;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Handle(RemoveWhereHosingCommand request, CancellationToken cancellationToken)
        {
            await _WhereHosingRepo.Delete(request.Id);
            await _WhereHosingRepo.Save();

        }
    }
}
