
using Hangfire;
using InventorySystem.API.MiddleWare;
using InventorySystem.Application.CQRS.Products.Commands;
using InventorySystem.Application.Helper.Mapper;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Domain.Models;
using InventorySystem.Infrastructure.Context;
using InventorySystem.Infrastructure.Repositories;
using InventorySystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;


namespace InventorySystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<GlobalErrorHandle>();

            // Add services to the container.
            #region Injection

            builder.Services.AddScoped(typeof(IGenericRepo<,>), typeof(GenericRepo<,>));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddDbContext<ApplicationContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();


            #endregion

            builder.Services.AddControllers();
            builder.Services.AddScoped<IStockCheckerService, StockCheckerService>();

            #region JWT

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy =>
                     policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
                  );
            });
          

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


            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AddProductCommandHandler).Assembly);
            });
            builder.Services.AddHangfire(config =>
 config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddHangfireServer();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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


            builder.Services.AddAutoMapper(
    typeof(ProductProfile).Assembly,
    typeof(CategoryProfile).Assembly
);



            var app = builder.Build();


            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();//System.ArgumentException: 'Cannot instantiate implementation type 'InventorySystem.Domain.Interfaces.IGenericRepo`1[T]' for service type 'InventorySystem.Domain.Interfaces.IGenericRepo`1[T]'.'

                app.UseSwaggerUI();
            //}
            app.UseMiddleware<GlobalErrorHandle>();

            app.UseAuthorization();
            app.UseHangfireDashboard("/dashboard");
            //RecurringJob.AddOrUpdate<IStockCheckerService>(
            //    "check-low-stock-test",
            //    service => service.CheckLowStockAsync(),
            //    Cron.Minutely());



            RecurringJob.AddOrUpdate<IStockCheckerService>(
    "check-low-stock-daily",
    service => service.CheckLowStockAsync(),
    Cron.Daily(1));

            app.MapControllers();

            app.Run();
        }
    }
}
