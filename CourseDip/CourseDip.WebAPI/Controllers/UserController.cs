using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseDip.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "admin,manager")]
        [HttpGet("instructors")] 
        public async Task<IActionResult> GetInstructors()
        {
            var instructors = await _userService.GetAllInstructorsAsync();
            return Ok(instructors);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var success = await _userService.RegisterUserAsync(userDto.FirstName,userDto.LastName, userDto.Email, userDto.Password);
            if (!success) return BadRequest("Email уже используется");
            return Ok("Пользователь зарегистрирован");
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateUserRoleDto updateRoleDto)
        {
            var success = await _userService.UpdateUserRoleAsync(id, updateRoleDto.Role);
            if (!success) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();
            return Ok("Пользователь удалён");
        }

    }
}