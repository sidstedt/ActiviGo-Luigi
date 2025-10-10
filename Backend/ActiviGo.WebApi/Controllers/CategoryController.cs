using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiviGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _category;
        public CategoryController(ICategoryService category)
        {
            _category = category;
        }

        //get api/category/withActivities
        [AllowAnonymous]
        [HttpGet]
        [Route("withActivities")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CategoryWithActivitiesDto>>> GetAllCategoriesWithActivities()
        {
            var categories = await _category.GetAllCategoriesWithActivities();
            return Ok(categories);
        }

        //get api/category
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _category.GetCategories();
            return Ok(categories);
        }

        //get api/category/{id}/activities
        [AllowAnonymous]
        [HttpGet]
        [Route("{id:int}/activities")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CategoryDto>> GetCategoryWithActivitiesById(int id)
        {
            var category = await _category.GetCategoryWithActivitiesById(id);
            return Ok(category);
        }

        //add activity to category
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("{categoryId:int}/activities/{activityId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddActivityToCategory(int categoryId, int activityId)
        {
            await _category.AddActivityToCategory(categoryId, activityId);
            return Ok("Added Successfully");
        }

        //remove activity from category
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{categoryId:int}/activities/{activityId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveActivityFromCategory(int categoryId, int activityId)
        {
            await _category.RemoveActivityFromCategory(categoryId, activityId);
            return Ok("Removed Successfully");
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPut]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //public async Task<IActionResult> CreateCategory( [FromBody] CreateCategoryDto dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var created = await _category.CreateAsync(dto);
        //    return Ok(created);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPut]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //public async Task<IActionResult> UpdateCategory(int id,[FromBody] CategoryUpdateDto dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var updated = await _category.UpdateAsync(id, dto);
        //    return Ok(updated);
        //}
    }
}
