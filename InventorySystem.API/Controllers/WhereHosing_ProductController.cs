
using InventorySystem.Application.Feature.WhereHosing_Products.Commands;
using InventorySystem.Application.DTOs.WhereHosingProductDTOs;
using InventorySystem.Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhereHosing_ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WhereHosing_ProductController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        #region add-wherehosing-product
        [HttpPost("add-wherehosing-product")]
        public async Task<IActionResult> AddWhereHosingProduct([FromBody] WhereHosingProductDTO command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new AddWhereHosing_ProductCommand(command));

            return result.ToActionResult();
        }

        #endregion

    }
}
