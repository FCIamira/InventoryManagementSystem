using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InventoryMangmentSystem.Domain.DTOs.Categories;
using InventoryMangmentSystem.Domain.Interfaces;
using InventoryMangmentSystem.Domain.Models;
using InventoryMangmentSystem.DAL.Data;

using MediatR;

namespace InventoryMangmentSystem.DAL.CQRS.Categories.Commands
{
    public class AddCategoryCommand :IRequest
    {
        public CategoryCreateDTO CategoryDto { get; set; }

        public AddCategoryCommand(CategoryCreateDTO dto)
        {
            CategoryDto = dto;
        }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand>
    {
        private IMapper _mapper;
       private IGenericRepo<Category> _CategoryRepo;

        public AddCategoryCommandHandler(IMapper mapper,IGenericRepo<Category> CategoryRepo)//,ICategoryRepo CategoryRepo) 
        {
            _mapper = mapper;
            _CategoryRepo = CategoryRepo;
        }   
        public async Task  Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {

            var Category = _mapper.Map<Category>(request.CategoryDto);
            await _CategoryRepo.Add(Category);

            await _CategoryRepo.Save();
            //return Unit.Value;
        }
    }
}
