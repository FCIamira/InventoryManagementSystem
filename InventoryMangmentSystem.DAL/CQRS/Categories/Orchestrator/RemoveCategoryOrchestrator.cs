using InventoryMangmentSystem.DAL.CQRS.Categories.Commands;
using InventoryMangmentSystem.DAL.CQRS.Products.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Categories.Orchestrator
{
    public class RemoveCategoryOrchestrator:IRequest
    {
        public int Id { get; set; }
       
    }

    public class RemoveCategoryOrchestratorHandler : IRequestHandler<RemoveCategoryOrchestrator>
    {
        private IMediator _mediator;

        public RemoveCategoryOrchestratorHandler(IMediator mediator) 
        {
            _mediator=mediator;
        }
        public async Task Handle(RemoveCategoryOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveCategoryCommand { Id = request.Id });

            await _mediator.Send(new RemoveProductCommand { Id = request.Id });
            

        }
    }
}
