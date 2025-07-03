using InventorySystem.Application.Feature.Categories.Commands;
using InventorySystem.Application.Feature.Products.Commands;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Feature.Categories.Orchestrator
{
    public class RemoveCategoryOrchestrator:IRequest<Result<string>>
    {
        public Guid Id { get; set; }
       
    }

    public class RemoveCategoryOrchestratorHandler : IRequestHandler<RemoveCategoryOrchestrator,Result<string>>
    {
        private IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveCategoryOrchestratorHandler(IMediator mediator,IUnitOfWork unitOfWork) 
        {
            _mediator=mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(RemoveCategoryOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveProductsByCategoryIdCommand { CategoryId = request.Id});
            await _mediator.Send(new RemoveCategoryCommand { Id = request.Id });
            await _unitOfWork.SaveChangesAsync();

            return Result<string>.Success("Category and its products removed successfully.");


        }
    }
}
