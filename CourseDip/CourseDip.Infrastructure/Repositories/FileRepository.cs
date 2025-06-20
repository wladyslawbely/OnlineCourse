using CourseDip.Application.Interfaces.Repository;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.Entities.File file)
        {
            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file != null)
            {
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Domain.Entities.File?> GetFileByIdAsync(int id)
        {
            return await _context.Files.FindAsync(id);
        }

        public async Task<IEnumerable<Domain.Entities.File>> GetFilesByAssignmentIdAsync(int assignmentId)
        {
            return await _context.Files
                 .Where(f => f.AssignmentId == assignmentId)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.File>> GetFilesByLessonIdAsync(int lessonId)
        {
            return await _context.Files
               .Where(f => f.LessonId == lessonId)
               .ToListAsync();
        }
    }
}
