﻿using InventorySystem.Application.Feature.Account.Orchestrator;
using InventorySystem.Application.DTOs.Account;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {

            this.mediator = mediator;
        }

        
        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Result<string>.Failure(ErrorCode.BadRequest, "Invalid model state").ToActionResult();
            }

            var result = await mediator.Send(new RegisterOrchestrator { RegisterRequest = model });
            return result.ToActionResult();
        }
        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Result<string>.Failure(ErrorCode.BadRequest, "Invalid model state").ToActionResult();
            }

            var result = await mediator.Send(new LoginOrchestrator { loginRequest = model });
            return result.ToActionResult();
        }
        #endregion

    }
}