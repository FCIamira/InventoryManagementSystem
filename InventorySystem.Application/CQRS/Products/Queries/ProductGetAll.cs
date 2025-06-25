using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventorySystem.Application.DTOs.ProductDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.DAL.CQRS.Products.Queries;
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

namespace InventorySystem.Application.CQRS.Products.Queries
{
    public class ProductGetAll : IRequest<Result<IEnumerable<ProductDTO>>>
    {

    }

    public class ProductGetAllHandler : IRequestHandler<ProductGetAll, Result<IEnumerable<ProductDTO>>> // Return IEnumerable
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly ILogger<ProductGetAllHandler> _logger;

        public ProductGetAllHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductGetAllHandler> _logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this._logger = _logger;
        }

        public async Task<Result<IEnumerable<ProductDTO>>> Handle(ProductGetAll request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving All ", request);

            try
            {
                var products = await _unitOfWork.Product.GetAll();
                if (products == null) 
                {
                    _logger.LogWarning("Products not found ", request);

                    return Result<IEnumerable<ProductDTO>>.Failure(ErrorCode.NotFound,"Product Not Found");              
                }
                _logger.LogInformation("Successfully retrieved products=", request);

                var dto =  _mapper.Map<IEnumerable<ProductDTO>>(products);
                return Result<IEnumerable<ProductDTO>>.Success(dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while retrieving products", request);

                return Result<IEnumerable<ProductDTO>>.Failure(ErrorCode.ServerError,
                    "An unexpected error occurred while retrieving the product.");
            }
        }

    }
}
