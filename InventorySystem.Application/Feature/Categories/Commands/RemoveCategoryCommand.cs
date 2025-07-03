using AutoMapper;
using InventorySystem.Application.Feature.Categories.Orchestrator;
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

namespace InventorySystem.Application.Feature.Categories.Commands
{
    public class RemoveCategoryCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }

    }

    public class RemoveCategoryCommandHandler : IRequestHandler<RemoveCategoryCommand, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveCategoryCommandHandler> _logger;

        public RemoveCategoryCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper, ILogger<RemoveCategoryCommandHandler> logger)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start removing category with ID = {CategoryId}", request.Id);

            try
            {
                if (request.Id == Guid.Empty)
                {
                    _logger.LogWarning("Empty category ID in request: {@Request}", request);
                    return Result<string>.Failure(ErrorCode.NotFound, "Category ID is required.");
                }

                var category = await _unitOfWork.Category.GetById(request.Id);
                if (category == null)
                {
                    _logger.LogWarning("Category with ID {CategoryId} not found", request.Id);
                    return Result<string>.Failure(ErrorCode.NotFound, "Category not found.");
                }

                await _unitOfWork.Category.Remove(request.Id);
                _logger.LogInformation("Successfully removed category with ID = {CategoryId}", request.Id);

                return Result<string>.Success("Category removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing category. Request: {@Request}", request);
                return Result<string>.Failure(ErrorCode.ServerError,
                   "An unexpected error occurred while removing the category.");
            }
        }
    }

}
