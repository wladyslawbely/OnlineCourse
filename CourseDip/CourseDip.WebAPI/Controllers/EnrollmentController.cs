using CourseDip.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseDip.Application.DTO;
using System.Security.Claims;

namespace CourseDip.WebAPI.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [Authorize(Roles = "student")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEnrollments(int userId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByUserIdAsync(userId);
            return Ok(enrollments);
        }

        [Authorize(Roles = "student")]
        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentDto enrollmentDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            enrollmentDto.UserId = userId;

            var success = await _enrollmentService.EnrollStudentAsync(enrollmentDto);
            if (!success)
                return BadRequest("Не удалось записаться на курс");

            return Ok("Запись на курс успешно выполнена");
        }

        [Authorize(Roles = "student")]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> Unenroll([FromBody] EnrollmentDto enrollmentDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var success = await _enrollmentService.UnenrollStudentAsync(enrollmentDto);
            if (!success)
                return BadRequest("Не удалось отписаться от курса");

            return Ok("Вы успешно отписались от курса");
        }

        [Authorize(Roles = "student")]
        [HttpPut("{userId}/{courseId}/progress")]
        public async Task<IActionResult> UpdateProgress(int userId, int courseId, [FromBody] decimal progress)
        {
            var result = await _enrollmentService.UpdateProgressAsync(userId, courseId, progress);
            if (!result) return NotFound("Запись о зачислении не найдена");

            return NoContent();
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> CheckEnrollment(int courseId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isEnrolled = await _enrollmentService.IsUserEnrolledAsync(userId, courseId);
            return Ok(new { isEnrolled });
        }
    }
}
