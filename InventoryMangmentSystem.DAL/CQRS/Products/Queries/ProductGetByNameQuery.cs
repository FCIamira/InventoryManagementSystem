using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryMangmentSystem.Domain.DTOs.Products;
using InventoryMangmentSystem.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Products.Queries
{
    public class ProductGetByNameQuery : IRequest<ProductDTO>
    {
        public string Name { get; set; }
    }


    public class ProductGetByNameQueryHandler : IRequestHandler<ProductGetByNameQuery, ProductDTO>
    {
        private IGenericRepo<Product> _productRepo;
        IMapper _mapper;
        public ProductGetByNameQueryHandler(IGenericRepo<Product> productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        public Task<ProductDTO> Handle(ProductGetByNameQuery request, CancellationToken cancellationToken)
        {
            var product = _productRepo.Get(x => x.Name.Contains(request.Name))
                .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)

                .FirstOrDefault();
            return Task.FromResult(product);
        }
    }
}
