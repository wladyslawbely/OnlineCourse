using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace CourseDip.Infrastructure.Repositories
{
    public class CourseCategoryRepository : Application.DTO.CourseCategoryDto, ICourseCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CourseCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Coursecategory category)
        {
            _context.Coursecategories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Coursecategories.FindAsync(id);
            if (category != null)
            {
                _context.Coursecategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Coursecategory>> GetAllAsync()
        {
            return await _context.Coursecategories.ToListAsync();
        }

        public async Task<Coursecategory?> GetByIdAsync(int id)
        {
            return await _context.Coursecategories.FindAsync(id);
        }

        public async Task UpdateAsync(Coursecategory category)
        {
            _context.Coursecategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Coursecategory>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Coursecategories
                .Where(c => ids.Contains(c.CategoryId))
                .ToListAsync();
        }
    }
}
