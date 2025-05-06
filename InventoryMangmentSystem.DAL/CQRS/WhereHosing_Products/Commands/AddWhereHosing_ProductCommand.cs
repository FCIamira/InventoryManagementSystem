using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using InventoryMangmentSystem.Domain.DTOs.WhereHosing_Product;
using InventoryMangmentSystem.Domain.Interfaces;

namespace InventoryMangmentSystem.DAL.CQRS.WhereHosing_Products.Commands
{
    public class AddWhereHosing_ProductCommand : IRequest
    {
        public WhereHosingProductDTO WhereHosingDto { get; set; }

        public AddWhereHosing_ProductCommand(WhereHosingProductDTO dto)
        {
            WhereHosingDto = dto;
        }
    }

    public class AddWhereHosing_ProductCommandHandler : IRequestHandler<AddWhereHosing_ProductCommand>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepo<WhereHosing_Product> _whereHosingRepo;

        public AddWhereHosing_ProductCommandHandler(IMapper mapper, IGenericRepo<WhereHosing_Product> whereHosingRepo)
        {
            _mapper = mapper;
            _whereHosingRepo = whereHosingRepo;
        }

        public async Task Handle(AddWhereHosing_ProductCommand request, CancellationToken cancellationToken)
        {
            var whereHosingProduct = _mapper.Map<WhereHosing_Product>(request.WhereHosingDto);

            await _whereHosingRepo.Add(whereHosingProduct);  

            await _whereHosingRepo.Save();  
        }
    }
}
