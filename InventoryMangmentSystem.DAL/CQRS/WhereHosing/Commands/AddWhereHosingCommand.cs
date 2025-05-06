using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InventoryMangmentSystem.Domain.DTOs.TransactionHistories;
using InventoryMangmentSystem.Domain.Interfaces;
using InventoryMangmentSystem.Domain.Models;
using InventoryMangmentSystem.DAL.Data;

using MediatR;
using InventoryMangmentSystem.Domain.DTOs.WhereHosing;

namespace InventoryMangmentSystem.DAL.CQRS.WhereHosings.Commands
{
    public class AddWhereHosingCommand :IRequest
    {
        public WhereHosingDTO WhereHosingDto { get; set; }

        public AddWhereHosingCommand(WhereHosingDTO dto)
        {
            WhereHosingDto = dto;
        }
    }

    public class AddWhereHosingCommandHandler : IRequestHandler<AddWhereHosingCommand>
    {
        private IMapper _mapper;
       private IGenericRepo<WhereHosing> _WhereHosingRepo;

        public AddWhereHosingCommandHandler(IMapper mapper,IGenericRepo<WhereHosing> WhereHosingRepo)
        {
            _mapper = mapper;
            _WhereHosingRepo = WhereHosingRepo;
        }   
        public async Task  Handle(AddWhereHosingCommand request, CancellationToken cancellationToken)
        {

            var WhereHosing = _mapper.Map<WhereHosing>(request.WhereHosingDto);
            await _WhereHosingRepo.Add(WhereHosing);

            await _WhereHosingRepo.Save();
            //return Unit.Value;
        }
    }
}
