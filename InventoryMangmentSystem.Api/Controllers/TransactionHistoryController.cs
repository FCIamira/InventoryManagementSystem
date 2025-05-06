using InventoryMangmentSystem.DAL.CQRS.Stocks.Queries;
using InventoryMangmentSystem.DAL.CQRS.TransactionHistory.Orchestrator;
using InventoryMangmentSystem.DAL.CQRS.TransactionsHistory.Commands;
using InventoryMangmentSystem.Domain.DTOs.TransactionHistories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMangmentSystem.Api.Controllers
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
        public async Task<IEnumerable<RemoveStockDTO>> GetAllStock()
        {
            var Stocks = await _mediator.Send(new StockGetAll());
            return Stocks;
        }

        #endregion

        #region Add
        [HttpPost]
        public async Task<IActionResult> StockCreate([FromBody] AddStockDTO StockDTO)
        {
            await _mediator.Send(new AddStockOrchestrator(StockDTO));
            return Ok(new { message = "Stock created successfully" });

        }
        #endregion

        #region Transfar
        [HttpPut("Transfar")]
        public async Task<IActionResult> EditStock(TransfarStockDTO StockEdit)
        {
            if (StockEdit == null || StockEdit.Quantity <= 0 || StockEdit.ProductID <= 0)
            {
                return BadRequest(new { message = "Invalid data provided" });
            }

            try
            {
                await _mediator.Send(new TransfarStockCommand(StockEdit));
                return Ok(new { message = "Stock Edit successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request", details = ex.Message });
            }
        }

#endregion

        #region Remove
        [HttpPost("RemoveStock")]
        public async Task<IActionResult> DeleteStock([FromBody] RemoveStockDTO StockDTO)
        {
            try
            {
                await _mediator.Send(new RemoveStockOrchestrator(StockDTO));

                return Ok(new { message = "Stock removed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "An error occurred while removing stock.", details = ex.Message });
            }
        }
        #endregion

    }
}
