using InventoryMangmentSystem.DAL.CQRS.Products.Queries.ProductsWithLowStockQuery;
using InventoryMangmentSystem.Domain.DTOs.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMangmentSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        IMediator mediator;
        public ReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        //Low Stock Report: List products below their LowStockThreshold.
        [HttpGet("LowStock")]
        public ActionResult GetproductsHasLowStockThreshold()
        {
            return Ok(mediator.Send(new ProductsWithLowStockQuery()));
        }

        //[HttpPost("TransactionHistory")]
        //public ActionResult TransactionHistory(TransactionHistoryDTO transactionHistory)
        //{
        //    var commond = MapperService.Map<TransactionHistoryQuery>(transactionHistory);
        //    return Ok(mediator.Send(commond));
        //}
    }
}
