using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task<Notification?> GetNotificationByIdAsync(int id);
        Task <int> CreateNotificationAsync(Notification notification);
        Task<Notification?> GetExistingDeadlineNotification(int userId, string assignmentTitle);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
    }
}
