using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.DTO;


namespace CourseDip.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);          
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<int> userIds)
        {
            return await _context.Users
                .Where(u => userIds.Contains(u.UserId))
                .ToListAsync();
        }
    }
}
