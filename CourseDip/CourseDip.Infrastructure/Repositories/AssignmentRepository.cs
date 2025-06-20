using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseDip.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsWithDeadlinesAsync(int userId, DateTime deadlineTime)
        {
            return await _context.Assignments
                .Where(a => a.DueDate.HasValue && a.DueDate <= deadlineTime) 
                .Where(a => _context.Enrollments.Any(e => e.CourseId == a.Lesson.Module.CourseId && e.UserId == userId)) 
                .ToListAsync();
        }

        public async Task AddAsync(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();           
        }

        public async Task DeleteAsync(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if(assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Assignment?> GetByIdAsync(int id)
        {
            return await _context.Assignments.FindAsync(id);
        }

        public async Task<IEnumerable<Assignment>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Assignments
                .Where(a => a.LessonId == lessonId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Assignment assignment)
        {
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
        }
    }
}
