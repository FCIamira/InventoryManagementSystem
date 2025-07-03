using InventorySystem.Application.Feature.Categories.Commands;
using InventorySystem.Application.Feature.Categories.Orchestrator;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Feature.Products.Commands
{
    public class RemoveProductsByCategoryIdCommand:IRequest<Result<string>>
    {
        public Guid CategoryId { get; set; }
    }

    public class RemoveProductsByCategoryIdHandler : IRequestHandler<RemoveProductsByCategoryIdCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<RemoveProductsByCategoryIdHandler> _logger;

        public RemoveProductsByCategoryIdHandler(IUnitOfWork unitOfWork,IMediator mediator,ILogger<RemoveProductsByCategoryIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(RemoveProductsByCategoryIdCommand request, CancellationToken cancellationToken)
        {
            try { 
            if (request.CategoryId == Guid.Empty)
            {
                return Result<string>.Failure("Category ID cannot be empty.");
            }

            var categoryExists = await _unitOfWork.Category.GetById(request.CategoryId);
            if (categoryExists == null)
            {
                return Result<string>.Failure($"Category with ID {request.CategoryId} not found.");
            }

            // Get products by category
            var products = await _unitOfWork.Product.GetProductByCategory(request.CategoryId);

            if (!products.Any())
            {
                return Result<string>.Success("No products found for the specified category.");
            }

            // Remove products
            foreach (var product in products)
            {
                _unitOfWork.Product.Remove(product.Id);
            }

            return Result<string>.Success($"Successfully removed {products.Count()} products from category {request.CategoryId}.");
        }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error removing products for category {CategoryId}", request.CategoryId);

                return Result<string>.Failure($"Failed to remove products: {ex.Message}");
            }

}
    }
}