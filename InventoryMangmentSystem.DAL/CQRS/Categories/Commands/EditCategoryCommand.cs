using AutoMapper;
using InventoryMangmentSystem.Domain.DTOs.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Categories.Commands
{
    public class EditCategoryCommand:IRequest
    {
        public CategoryEditDTO CategoryDto { get; set; }
        public EditCategoryCommand(CategoryEditDTO Category)
        {
            CategoryDto  =Category;
        }

    }
    public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand>
    {
        private readonly IGenericRepo<Category> _CategoryRepo;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EditCategoryCommandHandler(IMediator mediator, IGenericRepo<Category> CategoryRepo,IMapper mapper)
        {
            _CategoryRepo = CategoryRepo;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var Category = _mapper.Map<Category>(request.CategoryDto);
            await _CategoryRepo.Update(Category);
            await _CategoryRepo.Save();


        }
    }

}
