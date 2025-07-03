using AutoMapper;
using InventorySystem.Application.DTOs.ProductDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.DAL.Feature.Products.Queries;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.Feature.Products.Commands
{
    public class EditProductCommand:IRequest<Result<string>>
    {
        public ProductEditDTO ProductDto { get; set; }
        public EditProductCommand(ProductEditDTO product)
        {
            ProductDto  =product;
        }

    }
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand,Result<string>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductGetByNameQueryHandler> _logger;
        private IUnitOfWork _unitOfWork;
        public EditProductCommandHandler( IUnitOfWork unitOfWork,IMapper mapper, ILogger<ProductGetByNameQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<string>> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Edit product ", request);

            try
            {
                if (request.ProductDto == null)
                {
                    _logger.LogWarning("Product not found ", request);

                    return Result<string>.Failure(ErrorCode.BadRequest, "Product data is required.");
                }

                var product = _mapper.Map<Product>(request.ProductDto);
                
                await _unitOfWork.Product.Update(product.Id, product);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Successfully Edit product", request);

                return Result<string>.Success("Successfully Edit product");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Edit product }", request);

                return Result<string>.Failure(ErrorCode.ServerError,
                  "An unexpected error occurred while Edit the product.");

            }

        }
    }

}
