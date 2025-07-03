using InventoryMangmentSystem.DAL.Feature.Products.Commands;
using InventorySystem.Application.Feature.Products.Commands;
using InventorySystem.Application.Feature.Products.Queries;
using InventorySystem.Application.DTOs.ProductDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.DAL.Feature.Products.Queries;
using InventorySystem.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }


        #region GetByName


        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var product = await _mediator.Send(new ProductGetByNameQuery { Name = name });
            return  product.ToActionResult();
        }
        #endregion

        #region GetById


        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetProductByID(Guid id)
        {
            var Product = await _mediator.Send(new ProductGetByIdQuery { Id = id });
            return Product.ToActionResult();
        }
        #endregion

        #region GetAll

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _mediator.Send(new ProductGetAll());
            return products.ToActionResult();
        }

        #endregion

        #region Add
        [HttpPost]
        public async Task<IActionResult> ProductCreate([FromForm] ProductCreateDTO productDTO)
        {
             var result = await _mediator.Send(new AddProductCommand(productDTO));
            return result.ToActionResult();

        }
        #endregion


        #region Edit
        [HttpPut]
        public async Task<IActionResult> EditProduct([FromForm] ProductEditDTO productEdit)
        {
            var Result = await _mediator.Send(new EditProductCommand(productEdit));
            return Result.ToActionResult();
        }
        #endregion

        #region Remove
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
          var result=  await _mediator.Send(new RemoveProductCommand { Id = id });
            return result.ToActionResult();
        }
        #endregion

    }
}
