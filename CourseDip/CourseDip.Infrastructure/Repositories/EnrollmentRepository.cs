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
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Enrollment?> GetEnrollmentAsync(int userId, int courseId)
        {
            return await _context.Enrollments
               .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByUserIdAsync(int userId)
        {
            return await _context.Enrollments
              .Where(e => e.UserId == userId)
              .Include(e => e.Course)
              .ToListAsync();
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}
