using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseDip.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/submission")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [HttpGet("assignment/{assignmentId}")]
        public async Task<IActionResult> GetSubmissionsByAssignmentId(int assignmentId)
        {
            var submission = await _submissionService.GetSubmissionsByAssignmentIdAsync(assignmentId);
            return Ok(submission);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSubmissionsByUserId(int userId)
        {
            var submission = await _submissionService.GetSubmissionsByUserIdAsync(userId);
            return Ok(submission);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubmissionById(int id)
        {
            var submission = await _submissionService.GetSubmissionByIdAsync(id);
            if (submission == null) return NotFound("Работа не найдена");

            return Ok(submission);
        }

        [Authorize(Roles = "student")]
        [HttpPost]
        public async Task<IActionResult> CreateSubmission([FromBody] SubmissionDto submissionDto)

        {
            var submissionId = await _submissionService.CreateSubmissionAsync(submissionDto);
            return CreatedAtAction(nameof(GetSubmissionById), new { id = submissionId }, submissionDto);
        }

        [Authorize(Roles = "instructor,admin,student")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubmission(int id, [FromBody] SubmissionDto submissionDto)
        {
            var updated = await _submissionService.UpdateSubmissionAsync(id, submissionDto);
            if (!updated) return NotFound("Работа не найдена");

            return NoContent();
        }

        [Authorize(Roles = "instructor")]
        [HttpPut("{submissionId}/grade")]
        public async Task<IActionResult> GradeSubmission(int submissionId, [FromBody] GradeDto gradeDto)
        {
            var result = await _submissionService.GradeSubmissionAsync(submissionId, gradeDto.Grade, gradeDto.Feedback);
            if (!result) return NotFound("Работа не найдена");

            return NoContent();
        }


        [Authorize(Roles = "admin,student")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubmission(int id)
        {
            var deleted = await _submissionService.DeleteSubmissionAsync(id);
            if (!deleted) return NotFound("Работа не найдена");

            return NoContent();
        }

    }
}
