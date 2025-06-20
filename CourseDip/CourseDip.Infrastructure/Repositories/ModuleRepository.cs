using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseDip.Infrastructure.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext _context;

        public ModuleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Module module)
        {
           _context.Modules.Add(module);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
           var module = await _context.Modules.FindAsync(id);
            if (module != null)
            {
                _context.Modules.Remove(module);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Module>> GetAllAsync()
        {
            return await _context.Modules.ToListAsync();
        }

        public async Task<Module?> GetByIdAsync(int id)
        {
            return await _context.Modules.FindAsync(id);
        }

        public async Task<IEnumerable<Module>> GetModulesByCourseIdAsync(int courseId)
        {
            return await _context.Modules.Where(m  => m.CourseId == courseId).ToListAsync();
        }

        public async Task UpdateAsync(Module module)
        {
           _context.Modules.Update(module);
            await _context.SaveChangesAsync();
        }
    }
}
