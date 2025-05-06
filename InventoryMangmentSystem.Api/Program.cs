using InventoryMangmentSystem.Domain;
using InventoryMangmentSystem.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using InventoryMangmentSystem.DAL.Data;
using InventoryMangmentSystem.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using InventoryMangmentSystem.DAL.CQRS.Products.Queries;
using InventoryMangmentSystem.Domain.DTOs.Products;
using InventoryMangmentSystem.Domain.Interfaces;
namespace InventorySystem
{
    public class Program
    {
        //async Task
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //#region JWTToken

            builder.Services.AddDbContext<InventoryContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAutoMapper(typeof(ProductProfile)); // AutoMapper Profile

            #region Injection
            //builder.Services.AddScoped<IGenericRepo<T>, GenericRepo<T>>();

            
            builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

            builder.Services.AddControllers();
            #endregion

            builder.Services.AddControllers();

            #region Injection

            builder.Services.AddCors(options =>
            {
              options.AddPolicy("MyPolicy", policy =>
                   policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
                );
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<InventoryContext>().AddDefaultTokenProviders();

            //Setting Authanticatio  Middleware check using JWTToke
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;//check using jwt toke
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme; //redrect response in case not found cookie | token
                options.DefaultScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Iss"],//proivder
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Aud"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

            #endregion


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #region swagger
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 5 Web API",
                    Description = " ITI Projrcy"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                    });
            });

            #endregion


            #region AutoMapper



            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            #endregion

            #region AddMeditR

            
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ProductGetByNameQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });


            #endregion


            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = { "Admin", "User" };

                // Create roles if not exist
                foreach (var role in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        roleManager.CreateAsync(new IdentityRole(role));
                    }

                }

                // Create default admin if not exists
                string adminEmail = "admin@gmail.com";
                string adminUsername = "admin";
                string adminPassword = "Admin@123";

                var adminUser = await userManager.FindByNameAsync(adminUsername);
                if (adminUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = adminUsername,
                        Email = adminEmail
                    };

                    var result = await userManager.CreateAsync(user, adminPassword);
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            ////app.UseAuthentication();

            app.UseAuthorization();
            app.UseCors("MyPolicy");


            app.MapControllers();

            app.Run();
        }
    }
    //builder.Services.AddScoped<ICategoryRepo>();
    //builder.Services.AddScoped<IProductRepo>();
    //builder.Services.AddScoped<ITransaction_History>();
    //builder.Services.AddScoped<ITransaction_Type>();
    //builder.Services.AddScoped<IWhereHosing>();
    //builder.Services.AddScoped<IWhereHosing_Product>();
}
