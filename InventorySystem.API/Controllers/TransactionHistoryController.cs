
using InventorySystem.Application.CQRS.Stocks.Queries;
using InventorySystem.Application.CQRS.TransactionHistory.Orchestrator;
using InventorySystem.Application.CQRS.TransactionsHistory.Commands;
using InventorySystem.Application.DTOs.TransactionHistoryDTOs;
using InventorySystem.Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private IMediator _mediator;
        public TransactionHistoryController(IMediator mediator)
        {
            _mediator = mediator;

        }
        #region GetAll

        [HttpGet]
        public async Task<IActionResult> GetAllStock()
        {
            var Stocks = await _mediator.Send(new StockGetAll());
            return Stocks.ToActionResult();
        }

        #endregion

        #region Add
        [HttpPost("Add Stock")]
        public async Task<IActionResult> StockCreate([FromForm] AddStockDTO StockDTO)
        {
           var result = await _mediator.Send(new AddStockOrchestrator(StockDTO));
            return result.ToActionResult();

        }
        #endregion

        #region Transfar
        [HttpPut("Transfar")]
        public async Task<IActionResult> EditStock([FromForm] TransfarStockDTO StockEdit)
        {
             var result= await _mediator.Send(new TransfarStockCommand(StockEdit));
             return result.ToActionResult();
 
        }

#endregion

        #region Remove
        [HttpPost("RemoveStock")]
        public async Task<IActionResult> DeleteStock([FromForm] RemoveStockDTO StockDTO)
        {
            
               var result = await _mediator.Send(new RemoveStockOrchestrator(StockDTO));

                return result.ToActionResult();
            
            
        }
        #endregion

    }
}
