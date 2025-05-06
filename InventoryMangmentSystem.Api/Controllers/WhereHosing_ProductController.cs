using InventoryMangmentSystem.DAL.CQRS.WhereHosing_Products.Commands;
using InventoryMangmentSystem.Domain.DTOs.WhereHosing_Product;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMangmentSystem.Api.Controllers
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

        [HttpPost("add-wherehosing-product")]
        public async Task<IActionResult> AddWhereHosingProduct([FromBody] WhereHosingProductDTO command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            await _mediator.Send(new AddWhereHosing_ProductCommand(command));

            return Ok(new { message = "WhereHosing_Product added successfully" });
        }

    }
}
