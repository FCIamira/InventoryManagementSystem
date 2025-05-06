using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryMangmentSystem.Domain.DTOs.Products;
using InventoryMangmentSystem.DAL.Data;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Products.Queries
{
    public class ProductGetAll: IRequest<IEnumerable<ProductDTO>>
    {

    }

    public class ProductGetAllHandler : IRequestHandler<ProductGetAll, IEnumerable<ProductDTO>> // Return IEnumerable
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepo<Product> _productRepo;

        public ProductGetAllHandler(IGenericRepo<Product>  productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(ProductGetAll request, CancellationToken cancellationToken)
        {
            return await _productRepo.GetAll()
                                      .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
                                      .ToListAsync();
        }
    }

}
