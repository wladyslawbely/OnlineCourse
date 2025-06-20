using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseDip.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification?> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<int> CreateNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification.NotificationId;
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Notification?> GetExistingDeadlineNotification(int userId, string assignmentTitle)
        {
            return await _context.Notifications
               .Where(n => n.UserId == userId && n.Message.Contains($"Срок сдачи задания '{assignmentTitle}'"))
               .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}
