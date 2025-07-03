using InventorySystem.Application.DTOs.CategoryDTOs;
using InventorySystem.Application.Validators;
using InventorySystem.Application.Feature.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventorySystem.Application.Feature.Categories.Commands;
using InventorySystem.Application.Feature.Categories.Orchestrator;

namespace InventorySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;


        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }




        #region GetAll

        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var Categorys = await _mediator.Send(new CategoryGetAll());
            return Categorys.ToActionResult();
        }

        #endregion

        #region Add
        [HttpPost]
        public async Task<IActionResult> CategoryCreate([FromForm] CategoryCreateDTO CategoryDTO)
        {
            var result = await _mediator.Send(new AddCategoryCommand(CategoryDTO));
            return result.ToActionResult();

        }
        #endregion


        #region Edit
        [HttpPut]
        public async Task<IActionResult> EditCategory([FromForm] CategoryEditDTO CategoryEdit)
        {
           var result = await _mediator.Send(new EditCategoryCommand(CategoryEdit));
            return result.ToActionResult();
        }
        #endregion

        #region Remove
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
           var result = await _mediator.Send(new RemoveCategoryOrchestrator { Id = id });

            return result.ToActionResult();
        }

        #endregion


    }
}