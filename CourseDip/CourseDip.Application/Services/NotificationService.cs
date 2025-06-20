using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;

namespace CourseDip.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAssignmentService _assignmentService;

        public NotificationService(INotificationRepository notificationRepository, IAssignmentService assignmentService)
        {
            _notificationRepository = notificationRepository;
            _assignmentService = assignmentService;
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _notificationRepository.GetUserNotificationsAsync(userId);

            var notificationDtos = notifications.Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                UserId = n.UserId,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead
            }).ToList();

            notificationDtos.AddRange(await GenerateDeadlineNotifications(userId));

            return notificationDtos;
        }

        private async Task<IEnumerable<NotificationDto>> GenerateDeadlineNotifications(int userId)
        {
            var upcomingAssignments = await _assignmentService.GetAssignmentsWithUpcomingDeadlinesAsync(userId);
            var deadlineNotifications = new List<NotificationDto>();

            foreach (var assignment in upcomingAssignments)
            {
                var daysLeft = (assignment.DueDate - DateTime.UtcNow).Value.Days;
                if (daysLeft <= 3)
                {
                    var existingNotification = await _notificationRepository.GetExistingDeadlineNotification(userId, assignment.Title);

                    if (existingNotification == null) 
                    {
                        var newNotification = new Notification
                        {
                            UserId = userId,
                            Message = $"Срок сдачи задания '{assignment.Title}' истекает {assignment.DueDate.Value.ToShortDateString()}!",
                            IsRead = false,
                            CreatedAt = DateTime.UtcNow
                        };

                        var notificationId = await _notificationRepository.CreateNotificationAsync(newNotification);

                        var savedNotification = await _notificationRepository.GetNotificationByIdAsync(notificationId);

                        deadlineNotifications.Add(new NotificationDto
                        {
                            NotificationId = savedNotification.NotificationId,
                            UserId = savedNotification.UserId,
                            Message = savedNotification.Message,
                            IsRead = savedNotification.IsRead,
                            CreatedAt = savedNotification.CreatedAt
                        });
                    }
                }
            }

            return deadlineNotifications;
        }


        public async Task<NotificationDto?> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null) return null;

            return new NotificationDto
            {
                NotificationId = notification.NotificationId,
                UserId = notification.UserId,
                Message = notification.Message,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead
            };
        }

        public async Task<int> CreateNotificationAsync(NotificationDto notificationDto)
        {
            var notification = new Notification
            {
                UserId = notificationDto.UserId,
                Message = notificationDto.Message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            return await _notificationRepository.CreateNotificationAsync(notification);
        }

        public async Task MarkAsReadAsync(int id)
        {
            await _notificationRepository.MarkAsReadAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _notificationRepository.DeleteAsync(id);
        }
    }
}
