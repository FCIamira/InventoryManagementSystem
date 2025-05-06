using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Products.Commands
{

    public class RemoveProductCommand:IRequest
    {
        public int Id { get; set; }
       
    }
    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand>
    {
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RemoveProductCommandHandler(IMediator mediator, IGenericRepo<Product> productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            await _productRepo.Delete(request.Id);
            await _productRepo.Save();

        }
    }
}
