﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using InventoryMangmentSystem.Domain.DTOs;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using InventoryMangmentSystem.Domain.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;

    namespace HandmadeMarket.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AccountController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly IConfiguration config;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration config)
            {
                this.userManager = userManager;
                this.config = config;
            this.roleManager= roleManager;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterDTO userFromConsumer)
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = new ApplicationUser()
                    {
                        UserName = userFromConsumer.UserName,
                        Email = userFromConsumer.Email
                    };
                    IdentityResult result = await userManager.CreateAsync(user, userFromConsumer.Password);
                    if (result.Succeeded)
                    {
                        return Ok("Account Create Success");
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                return BadRequest(ModelState);
            }
            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginDTO userFromConsumer)
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await userManager.FindByNameAsync(userFromConsumer.UserName);
                    if (user != null)
                    {
                        bool found = await userManager.CheckPasswordAsync(user, userFromConsumer.Password);
                        if (found)
                        {
                            #region Create Token
                            string jti = Guid.NewGuid().ToString();
                            var userRoles = await userManager.GetRolesAsync(user);


                            List<Claim> claim = new List<Claim>();
                            claim.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                            claim.Add(new Claim(ClaimTypes.Name, user.UserName));
                            claim.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));
                            if (userRoles != null)
                            {
                                foreach (var role in userRoles)

                                {
                                    claim.Add(new Claim(ClaimTypes.Role, role));
                                }
                            }
                            //-----------------------------------------------
                            SymmetricSecurityKey signinKey =
                                new(Encoding.UTF8.GetBytes(config["JWT:Key"]));

                            SigningCredentials signingCredentials =
                                new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

                            JwtSecurityToken myToken = new JwtSecurityToken(
                                issuer: config["JWT:Iss"],
                                audience: config["JWT:Aud"],
                                expires: DateTime.Now.AddHours(1),
                                claims: claim,
                                signingCredentials: signingCredentials
                                );
                            return Ok(new
                            {
                                expired = DateTime.Now.AddHours(1),
                                token = new JwtSecurityTokenHandler().WriteToken(myToken)
                            });
                            #endregion
                        }
                    }
                    ModelState.AddModelError("", "Invalid Account");
                }
                return BadRequest(ModelState);
            }
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Role name is required");

            bool roleExists = await roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return BadRequest("Role already exists");

            IdentityResult result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
                return Ok($"Role '{roleName}' created successfully");

            return BadRequest(result.Errors);
        }


    }
}


