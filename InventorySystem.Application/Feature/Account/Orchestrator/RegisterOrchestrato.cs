using MediatR;
using InventorySystem.Application.DTOs.Account;
using InventorySystem.Application.Feature.Account.Commands;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventorySystem.Application.Feature.Account.Orchestrator
{

    public class RegisterchestratorResponse
    {
        public string Token { get; set; }
        public DateTime Expired { get; set; }
    }

    public class RegisterOrchestrator : IRequest<Result<RegisterchestratorResponse>>
    {
        public RegisterRequest RegisterRequest { get; set; }
    }

    public class RegisterOrchestratorHandler : IRequestHandler<RegisterOrchestrator, Result<RegisterchestratorResponse>>
    {
        private readonly IMediator mediator;

        public RegisterOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Result<RegisterchestratorResponse>> Handle(RegisterOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RegisterCommand
            {
                registerRequest = request.RegisterRequest
            });

            if (!result.IsSuccess)
                return Result<RegisterchestratorResponse>.Failure(result.Errorcode, result.Message);

           

            string token = await mediator.Send(new GenerateTokenCommand
            {
                user = result.Data.ApplicationUser,
                Expired = result.Data.Expired
            });

            return Result<RegisterchestratorResponse>.Success(new RegisterchestratorResponse
            {
                Token = token,
                Expired = result.Data.Expired
            });
        }

    }
}
