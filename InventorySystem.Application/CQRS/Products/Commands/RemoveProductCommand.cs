using AutoMapper;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.CQRS.Products.Commands
{

    public class RemoveProductCommand:IRequest<Result<string>>
    {
        public Guid Id { get; set; }
       
    }
    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveProductCommandHandler> _logger;

        public RemoveProductCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper,ILogger<RemoveProductCommandHandler> logger)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<string>> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.Product.Remove(request.Id);
                await _unitOfWork.SaveChangesAsync();
                return Result<string>.Success("Product removed successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the product with ID {ProductId}", request.Id);

                return Result<string>.Failure(
                    ErrorCode.ServerError,
                    "Failed to remove the product. Please try again later or contact support."
                );
            }

        }
    }
}
