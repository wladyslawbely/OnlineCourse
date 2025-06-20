using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Infrastructure.Repositories
{
    public class LessonProgressRepository : ILessonProgressRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonProgressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Lessonprogress progress)
        {
            await _context.Lessonprogresses.AddAsync(progress);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Lessonprogress>> GetByUserIdAsync(int userId)
        {
            return await _context.Lessonprogresses
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Lessonprogress?> GetByUserAndLessonAsync(int userId, int lessonId)
        {
            return await _context.Lessonprogresses.FirstOrDefaultAsync(lp => lp.UserId == userId && lp.LessonId == lessonId);
        }

        public async Task<int> CountCompletedLessonsAsync(int userId, int courseId)
        {
            return await _context.Lessonprogresses
                .Where(lp => lp.UserId == userId && lp.Lesson.Module.CourseId == courseId)
                .CountAsync();
        }


        public async Task UpdateAsync(Lessonprogress progress)
        {
            _context.Lessonprogresses.Update(progress);
            await _context.SaveChangesAsync();
        }
    }
}
