using AutoMapper;
using InventoryMangmentSystem.Domain.DTOs.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.CQRS.Products.Commands
{
    public class EditProductCommand:IRequest
    {
        public ProductEditDTO ProductDto { get; set; }
        public EditProductCommand(ProductEditDTO product)
        {
            ProductDto  =product;
        }

    }
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand>
    {
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EditProductCommandHandler(IMediator mediator, IGenericRepo<Product> productRepo,IMapper mapper)
        {
            _productRepo = productRepo;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.ProductDto);
            await _productRepo.Update(product);
            await _productRepo.Save();


        }
    }

}
