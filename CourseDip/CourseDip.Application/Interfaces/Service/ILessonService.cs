using CourseDip.Domain.Entities;
using CourseDip.Application.DTO;

namespace CourseDip.Application.Interfaces.Service
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonDto>> GetLessonsByModuleIdAsync(int moduleId);
        Task<LessonDto?> GetLessonByIdAsync(int id);
        Task<int> CreateLessonAsync(LessonDto lessonDto);
        Task<bool> UpdateLessonAsync(int id, LessonDto lessonDto);
        Task<bool> DeleteLessonAsync(int id);
    }
}
