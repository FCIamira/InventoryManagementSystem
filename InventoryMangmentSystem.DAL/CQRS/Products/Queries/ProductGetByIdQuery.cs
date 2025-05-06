using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryMangmentSystem.DAL.Data;
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
    public class ProductGetByIdQuery:IRequest<ProductDetailsDTO>
    {
        public int Id { get; set; }
    }

    public class ProductGetByIdQueryHandler: IRequestHandler<ProductGetByIdQuery , ProductDetailsDTO>
    {
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IMapper _mapper;


        public ProductGetByIdQueryHandler(IGenericRepo<Product>  productRepo,IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public  Task<ProductDetailsDTO> Handle(ProductGetByIdQuery request, CancellationToken cancellationToken)
        {
            var product =  _productRepo.GetByID(request.Id);
            if (product == null) return null;
            var dto = _mapper.Map<ProductDetailsDTO>(product);

            return Task.FromResult(dto);


        }
    }
}
