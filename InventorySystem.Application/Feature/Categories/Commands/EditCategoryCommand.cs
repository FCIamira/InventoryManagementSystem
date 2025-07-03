using AutoMapper;
using InventorySystem.Application.DTOs.CategoryDTOs;
using InventorySystem.Application.Validators;
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

namespace InventorySystem.Application.Feature.Categories.Commands
{
    public class EditCategoryCommand:IRequest<Result<string>>
    {
        public CategoryEditDTO CategoryDto { get; set; }
        public EditCategoryCommand(CategoryEditDTO Category)
        {
            CategoryDto  =Category;
        }

    }
    public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand,Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<EditCategoryCommandHandler> _logger;

        public EditCategoryCommandHandler(IMediator mediator, IUnitOfWork unitOfWork,IMapper mapper,ILogger<EditCategoryCommandHandler> logger)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<string>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Add Category :  ", request);

            try
            {
                var category = _mapper.Map<Category>(request.CategoryDto);
                if (category == null) 
                {
                    _logger.LogWarning("Category not found ", request);
                    return Result<string>.Failure(ErrorCode.NotFound, "Category Not Found");
                }
                await _unitOfWork.Category.Update(category.Id, category);
                await _unitOfWork.SaveChangesAsync();
                return Result<string>.Success("Category Edit successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Added Category", request);

                return Result<string>.Failure(ErrorCode.ServerError,
                    "An unexpected error occurred while retrieving the category.");
            }

        }
    }

}
