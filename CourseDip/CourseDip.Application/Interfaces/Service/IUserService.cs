using CourseDip.Application.DTO;
using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Service
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<InstructorDto>> GetAllInstructorsAsync();
        Task<bool> RegisterUserAsync(string firstName, string lastName, string email, string password);
        Task<bool> DeleteUserAsync(int id);
        Task<string?> LoginAsync(string email, string password);
        Task<bool> UpdateUserRoleAsync(int userId, string newRole);
    }
}
