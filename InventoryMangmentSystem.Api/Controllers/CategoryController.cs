
using InventoryMangmentSystem.DAL.CQRS.Categories.Commands;
using InventoryMangmentSystem.DAL.CQRS.Categories.Orchestrator;
using InventoryMangmentSystem.DAL.CQRS.Categories.Queries;
using InventoryMangmentSystem.Domain.DTOs.Categories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMangmentSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        //private readonly IProductRepo _productRepo;
        private readonly IMediator _mediator;


        public CategoryController(IMediator mediator)
        {
            // _productRepo = productRepo;
            _mediator = mediator;
        }




        #region GetAll

        [HttpGet]
        public async Task<IEnumerable<CategoryDTO>> GetAllCategory()
        {
            var Categorys = await _mediator.Send(new CategoryGetAll());
            return Categorys;
        }

        #endregion

        #region Add
        [HttpPost]
        public async Task<IActionResult> CategoryCreate([FromBody] CategoryCreateDTO CategoryDTO)
        {
            await _mediator.Send(new AddCategoryCommand(CategoryDTO));
            return Ok(new { message = "Category created successfully" });

        }
        #endregion


        #region Edit
        [HttpPut]
        public async Task<IActionResult> EditCategory(CategoryEditDTO CategoryEdit)
        {
            await _mediator.Send(new EditCategoryCommand(CategoryEdit));
            return Ok(new { message = "Category Edit successfully" });
        }
        #endregion

        #region Remove
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _mediator.Send(new RemoveCategoryOrchestrator { Id = id });

            return Ok(new { message = "Category removed" });
        }

        #endregion


    }
}