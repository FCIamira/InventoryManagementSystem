using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InventoryMangmentSystem.Domain.DTOs.Products;
using InventoryMangmentSystem.Domain.Interfaces;
using InventoryMangmentSystem.Domain.Models;
using InventoryMangmentSystem.DAL.Data;

using MediatR;

namespace InventoryMangmentSystem.DAL.CQRS.Products.Commands
{
    public class AddProductCommand :IRequest
    {
        public ProductCreateDTO ProductDto { get; set; }

        public AddProductCommand(ProductCreateDTO dto)
        {
            ProductDto = dto;
        }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
    {
        private IMapper _mapper;
       private IGenericRepo<Product> _productRepo;

        public AddProductCommandHandler(IMapper mapper,IGenericRepo<Product> productRepo)//,IProductRepo productRepo) 
        {
            _mapper = mapper;
            _productRepo = productRepo;
        }   
        public async Task  Handle(AddProductCommand request, CancellationToken cancellationToken)
        {

            var product = _mapper.Map<Product>(request.ProductDto);
            await _productRepo.Add(product);

            await _productRepo.Save();
            //return Unit.Value;
        }
    }
}
