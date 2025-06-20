using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseDip.WebAPI.Controllers
{
    [Authorize]
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto uploadDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var fileId = await _fileService.UploadFileAsync(uploadDto, userId);
            return CreatedAtAction(nameof(GetFileById), new { id = fileId }, uploadDto);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileById(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            if (file == null) return NotFound();
            return Ok(file);
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetFilesByLessonId(int lessonId)
        {
            var files = await _fileService.GetFilesByLessonIdAsync(lessonId);
            return Ok(files);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            await _fileService.DeleteFileAsync(id);
            return NoContent();
        }
    }
}
