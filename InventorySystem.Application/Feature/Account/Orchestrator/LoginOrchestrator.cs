﻿using InventorySystem.Application.Feature.Account.Commands;
using InventorySystem.Application.DTOs.Account;
using InventorySystem.Application.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Feature.Account.Orchestrator
{
    public class LoginOrchestrator : IRequest<Result<LoginOrchestratorResponse>>
    {
        public LoginRequest loginRequest { get; set; }
    }

    public class LoginOrchestratorResponse
    {
        public string Token { get; set; }
        public DateTime Expired { get; set; }
    }

    public class LoginOrchestratorHandler : IRequestHandler<LoginOrchestrator, Result<LoginOrchestratorResponse>>
    {
        private readonly IMediator mediator;

        public LoginOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Result<LoginOrchestratorResponse>> Handle(LoginOrchestrator request, CancellationToken cancellationToken)
        {
            var loginResult = await mediator.Send(new LoginCommand { loginRequest = request.loginRequest });

            if (!loginResult.IsSuccess)
                return Result<LoginOrchestratorResponse>.Failure(loginResult.Errorcode, loginResult.Message);

            var token = await mediator.Send(new GenerateTokenCommand
            {
                user = loginResult.Data.ApplicationUser,
                Expired = loginResult.Data.Expired
            }, cancellationToken);

            return Result<LoginOrchestratorResponse>.Success(new LoginOrchestratorResponse
            {
                Token = token,
                Expired = loginResult.Data.Expired
            }, "Login successful");
        }
    }
}
