using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryMangmentSystem.Domain.DTOs.Categories;
using InventoryMangmentSystem.DAL.Data;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Categories.Queries
{
    public class CategoryGetAll: IRequest<IEnumerable<CategoryDTO>>
    {

    }

    public class CategoryGetAllHandler : IRequestHandler<CategoryGetAll, IEnumerable<CategoryDTO>> // Return IEnumerable
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepo<Category> _CategoryRepo;

        public CategoryGetAllHandler(IGenericRepo<Category>  CategoryRepo, IMapper mapper)
        {
            _CategoryRepo = CategoryRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> Handle(CategoryGetAll request, CancellationToken cancellationToken)
        {
            return await _CategoryRepo.GetAll()
                                      .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
                                      .ToListAsync();
        }
    }

}
