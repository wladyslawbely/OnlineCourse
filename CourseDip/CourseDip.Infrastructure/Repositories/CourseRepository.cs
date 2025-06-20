using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CourseDip.Application.Interfaces.Repository;


namespace CourseDip.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                      .Include(c => c.Instructor) 
                      .Include(c => c.Modules)
                          .ThenInclude(m => m.Lessons)
                      .Include(c => c.Categories)
                      .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetByUserIdAsync(int userId)
        {
            return await _context.Courses
                     .Where(c => c.InstructorId == userId)
                     .Include(c => c.Modules)
                         .ThenInclude(m => m.Lessons)
                     .Include(c => c.Categories)
                     .ToListAsync();
        }
        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                    .Include(c => c.Modules)
                    .ThenInclude(m => m.Lessons)
                    .Include(c => c.Categories)
                    .Include(c => c.Instructor)
                    .FirstOrDefaultAsync(c => c.CourseId == id);
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Entry(course).Collection(c => c.Categories).Load();

            var newCategories = await _context.Coursecategories
                .Where(cat => course.Categories.Select(c => c.CategoryId).Contains(cat.CategoryId))
                .ToListAsync();

            course.Categories.Clear();

            foreach (var category in newCategories)
            {
                _context.Coursecategories.Attach(category);
                course.Categories.Add(category);
            }

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
    }
}
