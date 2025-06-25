using InventorySystem.Application.DTOs.Account;
using InventorySystem.Domain.Models;
using InventorySystem.Application.Validators;
using InventorySystem.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.CQRS.Account.Commands
{

    public class RegisterCommandResponse
    {
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime Expired { get; set; }
    }

    public class RegisterCommand : IRequest<Result<RegisterCommandResponse>>
    {
        public RegisterRequest registerRequest { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterCommandResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<RegisterCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.Users.AnyAsync(u => u.Email == request.registerRequest.EmailAddress))
            {
                return Result<RegisterCommandResponse>.Failure(ErrorCode.Unauthorized, "Email is already registered.");
            }

            if (await _userManager.Users.AnyAsync(u => u.UserName == request.registerRequest.UserName))
            {
                return Result<RegisterCommandResponse>.Failure(ErrorCode.Unauthorized, "UserName is already registered.");
            }

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = request.registerRequest.FirstName,
                LastName = request.registerRequest.LastName,
                UserName = request.registerRequest.UserName,
                Email = request.registerRequest.EmailAddress
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.registerRequest.Password);
            if (!result.Succeeded)
            {
                string errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<RegisterCommandResponse>.Failure(ErrorCode.Unauthorized, errorMessages);
            }

            string[] roles = new[] { "Admin", "Manager", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string assignedRole = await _userManager.Users.AnyAsync() ? "User" : "Admin";
            await _userManager.AddToRoleAsync(user, assignedRole);

            DateTime expired = DateTime.Now.AddHours(3);
            return Result<RegisterCommandResponse>.Success(new RegisterCommandResponse()
            {
                ApplicationUser = user,
                Expired = expired
            });
        }
    }
}
