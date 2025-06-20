using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
    
        }

        public async Task DeleteAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
        }

       
        public async Task<Lesson?> GetByIdAsync(int id)
        {
           return await _context.Lessons.FindAsync(id);
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Lesson>> GetLessonByModuleIdAsync(int moduleId)
        {
            return await _context.Lessons
                     .Where(l => l.ModuleId == moduleId)
                     .OrderBy(l => l.OrderIndex) 
                     .ToListAsync();
        }
        public async Task<int> CountLessonsInCourseAsync(int courseId)
        {
            return await _context.Lessons
                .Where(l => l.Module.CourseId == courseId)
                .CountAsync();
        }

        public async Task<List<int>> GetStudentIdsByLessonId(int lessonId)
        {
            return await _context.Enrollments
                .Join(_context.Lessons, e => e.CourseId, l => l.Module.CourseId, (e, l) => new { e.UserId, l.LessonId })
                .Where(x => x.LessonId == lessonId)
                .Select(x => x.UserId)
                .ToListAsync();
        }


    }
}
