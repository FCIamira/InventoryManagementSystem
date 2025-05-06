using InventoryMangmentSystem.DAL.CQRS.WhereHosings.Queries;
using InventoryMangmentSystem.DAL.CQRS.TransactionsHistory.Commands;
using InventoryMangmentSystem.Domain.DTOs.TransactionHistories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using InventoryMangmentSystem.Domain.DTOs.WhereHosing;
using InventoryMangmentSystem.DAL.CQRS.WhereHosings.Commands;

namespace InventoryMangmentSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhereHosingController : ControllerBase
    {
        private IMediator _mediator;

        public WhereHosingController(IMediator mediator) 
        {
            _mediator= mediator;
        }

        #region GetAll

        [HttpGet]
        public async Task<IEnumerable<WhereHosingDTO>> GetAllWhereHosing()
        {
            var WhereHosings = await _mediator.Send(new WhereHosingGetAll());
            return WhereHosings;
        }

        #endregion

        #region Add
        [HttpPost]
        public async Task<IActionResult> WhereHosingCreate([FromBody] WhereHosingDTO WhereHosingDTO)
        {
            await _mediator.Send(new AddWhereHosingCommand(WhereHosingDTO));
            return Ok(new { message = "WhereHosing created successfully" });

        }
        #endregion


        

        #region Remove
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWhereHosing(int id)
        {
            await _mediator.Send(new RemoveWhereHosingCommand { Id = id });
            return Ok(new { messaege = "WhereHosing remove" });
        }
        #endregion
    }
}
