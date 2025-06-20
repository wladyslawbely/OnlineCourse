using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseDip.WebAPI.Controllers
{

    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("my")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> GetMyCourses()
        {
            var instructorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var courses = await _courseService.GetCoursesByInstructorIdAsync(instructorId);
            return Ok(courses);
        }


        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [Authorize(Roles = "instructor,manager")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseDto courseDto)
        {
            if(string.IsNullOrWhiteSpace(courseDto.Title))
            {
                return BadRequest("Введите название курса");
            }

            var instructorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            
            var courseCreated = await _courseService.CreateCourseAsync(courseDto, instructorId);
            return CreatedAtAction(nameof(GetById),new { id = courseCreated.CourseId },courseCreated);
        }

        [Authorize(Roles = "instructor,manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseDto courseDto)
        {
            var success = await _courseService.UpdateCourseAsync(id,courseDto);
            if (!success) return NotFound($"Курс с id {id} не найден");

            return NoContent();
        }

        [Authorize(Roles = "instructor, manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _courseService.DeleteCourseAsync(id);
            if (!success) return NotFound($"Курс с id {id} не найден");

            return NoContent();
        }
    }
}