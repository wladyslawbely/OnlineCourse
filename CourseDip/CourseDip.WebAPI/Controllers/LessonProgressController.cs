using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseDip.WebAPI.Controllers
{
    [Route("api/lesson-progress")]
    [ApiController]
    [Authorize(Roles = "student")]
    public class LessonProgressController : ControllerBase
    {
        private readonly ILessonProgressService _lessonProgressService;
        private readonly IEnrollmentService _enrollmentService;

        public LessonProgressController(ILessonProgressService lessonProgressService, IEnrollmentService enrollmentService)
        {
            _lessonProgressService = lessonProgressService;
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<IActionResult> TrackProgress([FromBody] LessonProgressDto progressDto)
        {
            var isNew = await _lessonProgressService.TrackLessonProgressAsync(progressDto);
            return isNew ? Ok("Прогресс сохранен") : Ok("Прогресс обновлен");
        }

        [HttpGet("completed-lessons")]
        public async Task<IActionResult> GetCompletedLessons()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var completedLessons = await _lessonProgressService.GetCompletedLessonIdsByUserAsync(userId);
            return Ok(completedLessons); 
        }


        [Authorize(Roles = "student")]
        [HttpGet("{courseId}/progress")]
        public async Task<IActionResult> GetCourseProgress(int courseId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var progress = await _enrollmentService.CalculateCourseProgressAsync(userId, courseId);
            return Ok(new { progress });
        }
    }
}
