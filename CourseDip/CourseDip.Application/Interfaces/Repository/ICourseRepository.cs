using CourseDip.Domain.Entities;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetByUserIdAsync(int userId);
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
    }
}