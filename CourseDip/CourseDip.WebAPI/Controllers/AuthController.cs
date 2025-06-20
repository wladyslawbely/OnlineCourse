using Microsoft.AspNetCore.Mvc;
using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CourseDip.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var token = await _userService.LoginAsync(request.Email, request.Password);
            if (token == null)
                return Unauthorized("Ошибка в авторизации");

            return Ok(new { Token = token });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetCurrentUser()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest(new { message = "Некорректный ID пользователя" });
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            var userDto = new UserResponseDto
            {
                userId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(userDto);
        }
    }
}
