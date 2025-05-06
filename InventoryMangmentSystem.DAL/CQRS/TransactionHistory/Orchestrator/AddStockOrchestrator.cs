using InventoryMangmentSystem.DAL.CQRS.Products.Commands;
using InventoryMangmentSystem.DAL.CQRS.TransactionsHistory.Commands;
using InventoryMangmentSystem.Domain.DTOs.TransactionHistories;
using InventoryMangmentSystem.Domain.DTOs.Products;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryMangmentSystem.Domain.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using InventoryMangmentSystem.Domain.DTOs.WhereHosing;
using InventoryMangmentSystem.Domain.DTOs.WhereHosing_Product;
using InventoryMangmentSystem.DAL.CQRS.TransactionHistory.Orchestrator.Queries;
using InventoryMangmentSystem.DAL.CQRS.WhereHosings.Commands;
using InventoryMangmentSystem.DAL.CQRS.WhereHosing_Products.Commands;


namespace InventoryMangmentSystem.DAL.CQRS.TransactionHistory.Orchestrator
{

    public class AddStockOrchestrator :IRequest
    {
        public AddStockDTO StockDto { get; set; }
        

        public AddStockOrchestrator(AddStockDTO stockDto)
        {
            StockDto = stockDto;
        }
    }
    public class AddStockOrchestratorHandler : IRequestHandler<AddStockOrchestrator>
    {
        private IMediator _mediator;
        private IGenericRepo<Product> _productRepo;
        IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;


        public AddStockOrchestratorHandler(IMediator mediator, 
            IGenericRepo<Product> productRepo,IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager
            )
        {
            _mediator = mediator;
            _productRepo = productRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task Handle(AddStockOrchestrator request, CancellationToken cancellationToken)
        {

            var httpcontext = _httpContextAccessor.HttpContext;
            var user = await _userManager.GetUserAsync(httpcontext.User);

            if (user == null)
            {
                throw new Exception("UserId is missing in the claims");
            }
            if (request.StockDto.Transaction_Type_Id == 0)
            {
                request.StockDto.Transaction_Type_Id = 2;
            }


            var product = _productRepo.GetByID(request.StockDto.ProductID);
            var whereHosingFrom = _productRepo.GetByID(request.StockDto.FromWherehosing ?? 0);
            if(product == null)
{
                throw new Exception("Product not found");
            }

           
            var whereHosingTo = _productRepo.GetByID(request.StockDto.ToWherehosing ?? 0);


            var whereHosing_Products = await _mediator.Send(new GetWhereHosingProductQuery(
                    request.StockDto.ProductID, request.StockDto.FromWherehosing??0));

            if (product.WhereHosing_Products == null || !product.WhereHosing_Products.Any())
            {
                throw new Exception("No WhereHosing_Products found for the product");
            }
            if (product != null && whereHosing_Products!=null)
            {
                product.Quantity += request.StockDto.Quantity;
                whereHosing_Products.Quantity += request.StockDto.Quantity;

            }

            var transactionHistory = new Transaction_History
            {
                UserId = user.Id,
                ProductID = product.Id,
                Quantity = request.StockDto.Quantity,
                Transaction_Type_ID= request.StockDto.Transaction_Type_Id

            };
            var whereHosingAndProduct = new WhereHosing_Product
            {
                Product_Id = product.Id,
                WhereHosing_Id = whereHosingFrom.Id,
                Quantity = request.StockDto.Quantity,
            };

            //var dto = _mapper.Map<WhereHosingProductDTO>(whereHosingAndProduct);

            //await _mediator.Send(new AddWhereHosing_ProductCommand(dto));

            await _mediator.Send(new AddStockCommand(request.StockDto, user.Id));
         


        }


    }

    }

