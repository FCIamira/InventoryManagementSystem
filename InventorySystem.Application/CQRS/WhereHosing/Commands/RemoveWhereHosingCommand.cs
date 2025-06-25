using AutoMapper;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventorySystem.Application.CQRS.WhereHosings.Commands
{
    public class RemoveWhereHosingCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
    }

    public class RemoveWhereHosingCommandHandler : IRequestHandler<RemoveWhereHosingCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveWhereHosingCommandHandler> _logger;

        public RemoveWhereHosingCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveWhereHosingCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(RemoveWhereHosingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Attempting to remove WhereHosing with ID: {Id}", request.Id);
                  await _unitOfWork.WhereHosing.Remove(request.Id);
                //if (!success)
                //{
                //    _logger.LogWarning("Failed to remove WhereHosing with ID: {Id}", request.Id);
                //    return Result<string>.Failure(ErrorCode.NotFound, "WhereHosing not found.");
                //}

                await _unitOfWork.SaveChangesAsync();

                return Result<string>.Success("WhereHosing removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing WhereHosing with ID: {Id}", request.Id);
                return Result<string>.Failure(ErrorCode.ServerError, "An unexpected error occurred while removing WhereHosing.");
            }
        }
    }
}
