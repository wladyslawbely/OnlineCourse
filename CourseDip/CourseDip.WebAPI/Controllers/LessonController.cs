using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace CourseDip.WebAPI.Controllers
{
    [Route("api/lessons")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet("module/{moduleId}")]
        public async Task<IActionResult> GetLessonsByModule(int moduleId)
        {
            var lessions = await _lessonService.GetLessonsByModuleIdAsync(moduleId);
            return Ok(lessions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            if (lesson == null) return NotFound($"Урок с id {id} не найден.");
            return Ok(lesson);
        }

        [Authorize(Roles = "instructor,manager")]
        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromBody] LessonDto lessonDto)
        {
            var lessonId = await _lessonService.CreateLessonAsync(lessonDto);
            return CreatedAtAction(nameof(GetLessonById), new { id = lessonId }, lessonDto);
        }

        [Authorize(Roles = "instructor,manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] LessonDto lessonDto)
        {
            var updated = await _lessonService.UpdateLessonAsync(id, lessonDto);
            if (!updated) return NotFound($"Урок с id {id} не найден.");
            return NoContent();
        }

        [Authorize(Roles = "instructor,manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var deleted = await _lessonService.DeleteLessonAsync(id);
            if (!deleted) return NotFound($"Урок с id {id} не найден.");
            return NoContent();
        }

    }
}
