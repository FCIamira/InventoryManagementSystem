using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Application.DTOs.WhereHosingDTOs;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Application.Validators;
using InventorySystem.Application.CQRS.Categories.Queries;
using Microsoft.Extensions.Logging;
using InventorySystem.Application.DTOs.CategoryDTOs;
using InventorySystem.Domain.Enum;

namespace InventorySystem.Application.CQRS.WhereHosings.Queries
{
    public class WhereHosingGetAll: IRequest<Result<IEnumerable<GetAllWhereHosingDTO>>>
    {

    }

    public class WhereHosingGetAllHandler : IRequestHandler<WhereHosingGetAll, Result<IEnumerable<GetAllWhereHosingDTO>>> // Return IEnumerable
    {
        private readonly IMapper _mapper;
        private readonly ILogger<WhereHosingGetAllHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public WhereHosingGetAllHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<WhereHosingGetAllHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<GetAllWhereHosingDTO>>> Handle(WhereHosingGetAll request, CancellationToken cancellationToken)
        {
            try
            {
                var whereHosingEntities = await _unitOfWork.WhereHosing.GetAll();

                if (whereHosingEntities == null || !whereHosingEntities.Any())
                {
                    _logger.LogWarning("No WhereHosing records found.");
                    return Result<IEnumerable<GetAllWhereHosingDTO>>.Failure(ErrorCode.NotFound, "No WhereHosing records found.");
                }

                var whereHosingDtos = _mapper.Map<IEnumerable<GetAllWhereHosingDTO>>(whereHosingEntities);

                return Result<IEnumerable<GetAllWhereHosingDTO>>.Success(whereHosingDtos);
            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving WherHosing", request);

                return Result<IEnumerable<GetAllWhereHosingDTO>>.Failure(ErrorCode.ServerError,
                   "An unexpected error occurred while retrieving the WhereHosing.");
            }
        }
    }

}
