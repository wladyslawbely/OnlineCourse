using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface ILessonProgressRepository
    {
        Task<Lessonprogress?> GetByUserAndLessonAsync(int userId, int lessonId);
        Task<IEnumerable<Lessonprogress>> GetByUserIdAsync(int userId);
        Task<int> CountCompletedLessonsAsync(int userId, int courseId);
        Task AddAsync(Lessonprogress progress);
        Task UpdateAsync(Lessonprogress progress);
    }
}
