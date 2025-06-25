using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Application.DTOs.CategoryDTOs;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.CQRS.Categories.Queries
{
    public class CategoryGetAll: IRequest<Result<IEnumerable<CategoryDTO>>>
    {

    }

    public class CategoryGetAllHandler : IRequestHandler<CategoryGetAll, Result<IEnumerable<CategoryDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryGetAllHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryGetAllHandler(IUnitOfWork unitOfWork, IMapper mapper,ILogger<CategoryGetAllHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CategoryDTO>>> Handle(CategoryGetAll request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving All ", request);

            try
            {
                var categories = await _unitOfWork.Category.GetAll();
                if (categories == null)
                {
                    _logger.LogWarning("Category not found ", request);

                    return Result<IEnumerable<CategoryDTO>>.Failure(ErrorCode.NotFound, "Categories Not Found");
                }
                var mappedCategories = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

                return Result<IEnumerable<CategoryDTO>>.Success(mappedCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Category", request);

                return Result<IEnumerable<CategoryDTO>>.Failure(ErrorCode.ServerError,
                   "An unexpected error occurred while retrieving the category.");
            }
                                     
        }
    }

}
