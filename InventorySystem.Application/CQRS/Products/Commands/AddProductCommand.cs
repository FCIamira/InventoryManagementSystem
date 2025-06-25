using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InventorySystem.Application.DTOs.ProductDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;

using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.CQRS.Products.Commands
{
    public class AddProductCommand : IRequest<Result<ProductDTO>>
    {
        public ProductCreateDTO ProductDto { get; set; }

        public AddProductCommand(ProductCreateDTO dto)
        {
            ProductDto = dto;
        }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result<ProductDTO>>
    {
        private IMapper _mapper;
       private IUnitOfWork _unitOfWork;
        private readonly ILogger<AddProductCommandHandler> _logger;

        public AddProductCommandHandler(IMapper mapper, IUnitOfWork unitOfWork,ILogger<AddProductCommandHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }   
        public async Task<Result<ProductDTO>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Add product ", request);

            try
            {
                var product = _mapper.Map<Product>(request.ProductDto);
                if (product == null)
                {
                    _logger.LogWarning("Product not found ", request);

                    return Result<ProductDTO>.Failure(ErrorCode.Conflict, $"Product with name '{request.ProductDto.Name}' already exists.");
                }
                await _unitOfWork.Product.Add(product);

                await _unitOfWork.SaveChangesAsync();
                var resultDto = _mapper.Map<ProductDTO>(product);

                _logger.LogInformation("Successfully Added product ", request);
                return Result<ProductDTO>.Success(
                 resultDto,
                 "Product created successfully."
             );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Added product ", request);

                return Result<ProductDTO>.Failure(ErrorCode.ServerError, "Database update failed. Possibly duplicate key or constraint violation.");
            }
            }
        }
}
