using AutoMapper;
using InventorySystem.Application.DTOs.CategoryDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.Application.CQRS.Categories.Commands
{
    public class AddCategoryCommand :IRequest<Result<string>>
    {
        public CategoryCreateDTO CategoryDto { get; set; }

        public AddCategoryCommand(CategoryCreateDTO dto)
        {
            CategoryDto = dto;
        }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand,Result<string>>
    {
        private IMapper _mapper;
       private IUnitOfWork _unitOfWork;
        private readonly ILogger<AddCategoryCommandHandler> _logger;

        public AddCategoryCommandHandler(IMapper mapper,IUnitOfWork unitOfWork,ILogger<AddCategoryCommandHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }   
        public async Task<Result<string>>  Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Added Category ", request);

            try
            {
                var category = _mapper.Map<Category>(request.CategoryDto);
                if (category == null)
                {
                    _logger.LogWarning("Category not found ", request);

                    Result<string>.Failure(ErrorCode.NotFound, "Category Not Found");
                }
                await _unitOfWork.Category.Add(category);

                await _unitOfWork.SaveChangesAsync();
                return Result<string>.Success("Category Added successfully");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while Added Category", request);

                return Result<string>.Failure(ErrorCode.ServerError,
                   "An unexpected error occurred while Added the category.");
            }
        }
    }
}
