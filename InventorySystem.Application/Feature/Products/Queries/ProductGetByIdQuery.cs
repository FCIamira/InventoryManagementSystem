using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventorySystem.Application.DTOs.ProductDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Feature.Products.Queries
{
    public class ProductGetByIdQuery:IRequest<Result<ProductDetailsDTO>>
    {
        public Guid Id { get; set; }
    }

    public class ProductGetByIdQueryHandler: IRequestHandler<ProductGetByIdQuery , Result<ProductDetailsDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public ProductGetByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public  async Task<Result<ProductDetailsDTO>> Handle(ProductGetByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Product.GetById(request.Id);
            try
            {

                if (product == null)
                { 
                    return Result<ProductDetailsDTO>.Failure(ErrorCode.NotFound, "Product not found"); 
                }
                var dto = _mapper.Map<ProductDetailsDTO>(product);

                return Result<ProductDetailsDTO>.Success(dto);
            }
            catch (Exception ex) {
                return Result<ProductDetailsDTO>.Failure(ErrorCode.ServerError,
                   $"An unexpected error occurred while retrieving the product.");

            }


        }
    }
}
