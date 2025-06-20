using CourseDip.Domain.Entities;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Application.DTO;

namespace CourseDip.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUserAsync(string firstName, string lastName, string email, string password)
        {
            if(await _userRepository.GetByEmailAsync(email) != null) return false;

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "student"
            };

            await _userRepository.AddAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<InstructorDto>> GetAllInstructorsAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users
                .Where(u => u.Role == "instructor")
                .Select(u => new InstructorDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).ToList();
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null; 
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            user.LastLogin = DateTime.UtcNow.ToLocalTime();
            await _userRepository.UpdateAsync(user);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
           return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }


        public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Role = newRole;
            await _userRepository.UpdateAsync(user);
            return true;
        }

    }
}
