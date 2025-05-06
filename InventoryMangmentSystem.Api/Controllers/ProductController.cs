using InventoryMangmentSystem.DAL.CQRS.Products.Queries;
using InventoryMangmentSystem.DAL.CQRS.Products.Commands;


using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryMangmentSystem.Domain.DTOs.Products;
using InventoryMangmentSystem.DAL.CQRS.TransactionsHistory.Commands;

namespace InventoryMangmentSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //private readonly IProductRepo _productRepo;
        private readonly IMediator _mediator;


        public ProductController( IMediator mediator)
        {
           // _productRepo = productRepo;
            _mediator = mediator;
        }
        #region GetByName

       
        [HttpGet("{name}")]
        public async Task<ProductDTO> GetByName(string name)
        {
            var product = await _mediator.Send(new ProductGetByNameQuery { Name = name });
            
                return product;
           
        }
        #endregion

        #region GetById

       
        [HttpGet("{id:int}")]
        public async Task<ProductDetailsDTO> GetProductByID(int id)
        {
            var Product = await _mediator.Send(new ProductGetByIdQuery { Id = id});
            return Product;
        }
        #endregion

        #region GetAll
       
        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> GetAllProduct()
        {
            var products = await _mediator.Send(new ProductGetAll());
            return products;
        }

        #endregion

        #region Add
        [HttpPost]
        public async Task<IActionResult>  ProductCreate([FromBody] ProductCreateDTO productDTO)
        {
            await _mediator.Send(new AddProductCommand(productDTO));
            return Ok(new { message = "Product created successfully" });

        }
        #endregion


        #region Edit
        [HttpPut]
        public async Task<IActionResult> EditProduct(ProductEditDTO productEdit)
        {
            await _mediator.Send(new EditProductCommand(productEdit));
            return Ok(new { message = "Product Edit successfully" });
        }
        #endregion

        #region Remove
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _mediator.Send(new RemoveProductCommand { Id = id });
            return Ok(new { messaege = "product remove" });
        }
        #endregion



     
    }
}
