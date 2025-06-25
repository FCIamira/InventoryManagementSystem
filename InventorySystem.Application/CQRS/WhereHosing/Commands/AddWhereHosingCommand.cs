using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InventorySystem.Application.DTOs.WhereHosingDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventorySystem.DAL.CQRS.WhereHosings.Commands
{
    public class AddWhereHosingCommand :IRequest<Result<string>>
    {
        public AddWhereHosingDTO WhereHosingDto { get; set; }

        public AddWhereHosingCommand(AddWhereHosingDTO dto)
        {
            WhereHosingDto = dto;
        }
    }

    public class AddWhereHosingCommandHandler : IRequestHandler<AddWhereHosingCommand,Result<string>>    
    {
        private IMapper _mapper;
       private IUnitOfWork _unitOfWork;
        private readonly ILogger<AddWhereHosingCommandHandler> _logger;

        public AddWhereHosingCommandHandler(IMapper mapper, IUnitOfWork unitOfWork,ILogger<AddWhereHosingCommandHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }   
        public async Task<Result<string>>  Handle(AddWhereHosingCommand request, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Adding new WhereHosing: {@WhereHosingDto}", request.WhereHosingDto);

                var whereHosing = _mapper.Map<WhereHosing>(request.WhereHosingDto);

                await _unitOfWork.WhereHosing.Add(whereHosing);
                await _unitOfWork.SaveChangesAsync();

                return Result<string>.Success("WhereHosing added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding WhereHosing.");
                return Result<string>.Failure(ErrorCode.ServerError, "An unexpected error occurred while adding WhereHosing.");
            }
        }
    }
}
