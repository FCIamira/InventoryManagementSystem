using System;
using System.Threading.Tasks;
using AutoMapper;
using InventorySystem.Application.DTOs.WhereHosingProductDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;


namespace InventorySystem.Application.CQRS.WhereHosing_Products.Commands
{
    public class AddWhereHosing_ProductCommand : IRequest<Result<string>>
    {
        public WhereHosingProductDTO WhereHosingDto { get; set; }

        public AddWhereHosing_ProductCommand(WhereHosingProductDTO dto)
        {
            WhereHosingDto = dto;
        }
    }

    public class AddWhereHosing_ProductCommandHandler : IRequestHandler<AddWhereHosing_ProductCommand,Result<string>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddWhereHosing_ProductCommandHandler> _logger;

        public AddWhereHosing_ProductCommandHandler(IMapper mapper, IUnitOfWork unitOfWork,ILogger<AddWhereHosing_ProductCommandHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(AddWhereHosing_ProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Added WhereHosing Product :  ", request);

            try
            {
                var whereHosingProduct = _mapper.Map<WhereHosing_Product>(request.WhereHosingDto);
                if (whereHosingProduct == null)
                {
                    _logger.LogWarning("Category not found ", request);
                    return Result<string>.Failure(ErrorCode.NotFound, "whereHosingProduct Not Found");
                }

                await _unitOfWork.WhereHosing_Product.Add(whereHosingProduct);

                await _unitOfWork.SaveChangesAsync();
                return Result<string>.Success(whereHosingProduct.Id.ToString(), "WhereHosing product added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Category", request);

                return Result<string>.Failure(ErrorCode.ServerError,
                   "An unexpected error occurred while retrieving the category.");
            }
        }
    }
}
