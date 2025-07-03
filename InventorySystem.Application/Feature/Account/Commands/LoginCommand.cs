using InventorySystem.Application.DTOs.Account;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using InventorySystem.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Feature.Account.Commands
{
    public class LoginCommand : IRequest<Result<LoginCommandResponse>>
    {
        public LoginRequest loginRequest { get; set; }
    }

    public class LoginCommandResponse
    {
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime Expired { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.loginRequest.EmailAddress);
            if (user == null)
            {
                return Result<LoginCommandResponse>.Failure(ErrorCode.NotFound, "User not found");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.loginRequest.Password);
            if (!result)
            {
                return Result<LoginCommandResponse>.Failure(ErrorCode.Unauthorized, "Incorrect password");
            }

            DateTime expireDate = request.loginRequest.RememberMe ? DateTime.Now.AddDays(1) : DateTime.Now.AddHours(3);
            return Result<LoginCommandResponse>.Success(new LoginCommandResponse
            {
                ApplicationUser = user,
                Expired = expireDate
            }, "Login successful");
        }
    }


}



