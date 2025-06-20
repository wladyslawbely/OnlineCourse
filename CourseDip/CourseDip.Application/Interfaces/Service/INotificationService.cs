using CourseDip.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Service
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId);
        Task<NotificationDto?> GetNotificationByIdAsync(int id);
        Task<int> CreateNotificationAsync(NotificationDto notificationDto);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
    }
}
