using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventorySystem.Application.DTOs.ProductDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.DAL.CQRS.Products.Queries
{
    public class ProductGetByNameQuery : IRequest<Result<ProductDTO>>
    {
        public string Name { get; set; }
    }


    public class ProductGetByNameQueryHandler : IRequestHandler<ProductGetByNameQuery, Result<ProductDTO>>
    {
        private IGenericRepo<Product,Guid> _productRepo;
        IMapper _mapper;
        private readonly ILogger<ProductGetByNameQueryHandler> _logger;

        public ProductGetByNameQueryHandler(IGenericRepo<Product, Guid> productRepo, IMapper mapper, ILogger<ProductGetByNameQueryHandler> logger)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<ProductDTO>> Handle(ProductGetByNameQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving product with Name: {ProductName}", request.Name); 
            try
            {

                var product = _productRepo.GetAllWithFilter(x => x.Name.Contains(request.Name))
                    .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefault();
                if (product == null)
                {
                    _logger.LogWarning("Product not found for name: {Name}", request.Name);

                    return Result<ProductDTO>.Failure(ErrorCode.NotFound, "Product not found ");
                }
                _logger.LogInformation("Successfully retrieved product with Name: {ProductName}", request.Name);
                return Result<ProductDTO>.Success(product);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while retrieving product with Name: {ProductName}", request.Name);
                return Result<ProductDTO>.Failure(
                    ErrorCode.ServerError,
                    "An unexpected error occurred while retrieving the product."
                );
            }
        }



    }


}
