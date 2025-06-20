using CourseDip.Application.DTO;
using CourseDip.Application.Services;
using Microsoft.AspNetCore.Mvc;
using CourseDip.Application.Interfaces.Service;
using System.Security.Claims;

namespace CourseDip.WebAPI.Controllers
{

    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentDto commentDto)
        {
            if (commentDto.LessonId == null && commentDto.CourseId == null)
            {
                return BadRequest("Комментарий должен быть привязан либо к уроку, либо к курсу.");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var commentId = await _commentService.CreateCommentAsync(commentDto, userId);

            if (commentDto.LessonId != null)
            {
                return CreatedAtAction(nameof(GetCommentsByLesson), new { lessonId = commentDto.LessonId }, commentId);
            }
            else
            {
                return CreatedAtAction(nameof(GetCommentsByCourse), new { courseId = commentDto.CourseId }, commentId);
            }
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetCommentsByLesson(int lessonId)
        {
            var comments = await _commentService.GetCommentsByLessonIdAsync(lessonId);
            return Ok(comments);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCommentsByCourse(int courseId)
        {
            var comments = await _commentService.GetCommentsByCourseIdAsync(courseId);
            return Ok(comments);
        }

        [HttpPost("reply/{parentCommentId}")]
        public async Task<IActionResult> ReplyToComment(int parentCommentId, [FromBody] CommentDto replyDto)
        {
            replyDto.ParentCommentId = parentCommentId;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var replyId = await _commentService.CreateCommentAsync(replyDto, userId);
            return CreatedAtAction(nameof(GetCommentsByCourse), new { courseId = replyDto.CourseId }, replyId);
        }



        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            await _commentService.DeleteCommentAsync(commentId);
            return NoContent();
        }
    }
}
