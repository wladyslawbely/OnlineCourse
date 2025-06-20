using CourseDip.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface ILessonRepository
    {
        Task<Lesson?> GetByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetLessonByModuleIdAsync(int moduleId);
        Task<int> CountLessonsInCourseAsync(int courseId);
        Task<List<int>> GetStudentIdsByLessonId(int lessonId);
        Task AddAsync(Lesson lesson);
        Task DeleteAsync(int id);
        Task UpdateAsync(Lesson lesson);

    }
}
