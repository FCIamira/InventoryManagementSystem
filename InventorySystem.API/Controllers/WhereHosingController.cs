
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using InventorySystem.Application.DTOs.WhereHosingDTOs;
using InventorySystem.Application.CQRS.WhereHosings.Queries;
using InventorySystem.Application.Validators;
using InventorySystem.DAL.CQRS.WhereHosings.Commands;
using InventorySystem.Application.CQRS.WhereHosings.Commands;

namespace InventorySystem.Api.Controllers
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
        public async Task<IActionResult> GetAllWhereHosing()
        {
            var WhereHosings = await _mediator.Send(new WhereHosingGetAll());
            return WhereHosings.ToActionResult();
        }

        #endregion

        #region Add
        [HttpPost]
        public async Task<IActionResult> WhereHosingCreate([FromForm] AddWhereHosingDTO WhereHosingDTO)
        {
            var result = await _mediator.Send(new AddWhereHosingCommand(WhereHosingDTO));
            return result.ToActionResult();

        }
        #endregion


        

        #region Remove
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteWhereHosing(Guid id)
        {
            var result =  await _mediator.Send(new RemoveWhereHosingCommand { Id = id });
            return  result.ToActionResult();
        }
        #endregion
    }
}
