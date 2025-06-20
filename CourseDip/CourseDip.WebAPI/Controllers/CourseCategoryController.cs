using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseDip.WebAPI.Controllers
{
    [Route("api/course-categories")]
    [ApiController]
    [Authorize(Roles = "admin, instructor,manager")]
    public class CourseCategoryController : ControllerBase
    {
        private readonly ICourseCategoryService _service;

        public CourseCategoryController(ICourseCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Coursecategory>>> GetCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Coursecategory>> GetCategory(int id)
        {
            var category = await _service.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateCategory([FromBody] CourseCategoryDto courseCategoryDto)
        {
            var categoryId = await _service.CreateCategoryAsync(courseCategoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = categoryId }, categoryId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CourseCategoryDto courseCategoryDto)
        {
            var result = await _service.UpdateCategoryAsync(id, courseCategoryDto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _service.DeleteCategoryAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
