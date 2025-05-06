using AutoMapper;
using InventoryMangmentSystem.DAL.CQRS.Categories.Commands;
using InventoryMangmentSystem.DAL.CQRS;
using InventoryMangmentSystem.DAL.CQRS.Products.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Categories.Commands
{
    public class RemoveCategoryCommand : IRequest
    {
        public int Id { get; set; }

    }

    public class RemoveCategoryCommandHandler : IRequestHandler<RemoveCategoryCommand>
    {
        private readonly IGenericRepo<Category> _CategoryRepo;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RemoveCategoryCommandHandler(IMediator mediator, IGenericRepo<Category> CategoryRepo, IMapper mapper)
        {
            _CategoryRepo = CategoryRepo;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
        {
            await _CategoryRepo.Delete(request.Id);

            await _mediator.Publish(new CategoryRemovedEvent { Id = request.Id });


        }
    }
}
