using CourseDip.Domain.Entities;
using File = CourseDip.Domain.Entities.File;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface IFileRepository
    {
        Task<IEnumerable<File>> GetFilesByLessonIdAsync(int lessonId);
        Task<IEnumerable<File>> GetFilesByAssignmentIdAsync(int assignmentId);
        Task<File?> GetFileByIdAsync(int id);
        Task AddAsync(File file);
        Task DeleteAsync(int id);
    }
}
