using Microsoft.AspNetCore.Mvc;
using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CourseDip.WebAPI.Controllers
{

    [Route("api/assignments")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetByLessonId(int lessonId)
        {
            var assignment = await _assignmentService.GetAssignmentsByLessonIdAsync(lessonId);
            return Ok(assignment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
            if (assignment == null) return NotFound("Задание не найдено");
            return Ok(assignment);
        }

        [Authorize(Roles = "instructor")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssignmentDto assignmentDto)
        {
            if (string.IsNullOrWhiteSpace(assignmentDto.Title))
            {
                return BadRequest("Название задания не может быть пустым");
            }

            var assignmentId = await _assignmentService.CreateAssignmentAsync(assignmentDto);

            return CreatedAtAction(nameof(GetById), new { id = assignmentId }, assignmentDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> Update(int id, [FromBody] AssignmentDto assignmentDto)
        {
            var assignment = await _assignmentService.UpdateAssignmentAsync(id, assignmentDto);
            if (!assignment) return BadRequest("Задание не найдено");
            return NoContent();
        }

        [Authorize(Roles = "instructor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _assignmentService.DeleteAssignmentAsync(id);
            if (!result) return NotFound("Задание не найдено");

            return NoContent();
        }
    }
}
