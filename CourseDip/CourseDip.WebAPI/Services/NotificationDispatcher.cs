using CourseDip.Application.Interfaces.Service;
using CourseDip.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CourseDip.WebAPI.Services
{
    public class NotificationDispatcher : INotificationDispatcher
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationDispatcher(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationAsync(int userId, string message)
        {
            var notification = new { UserId = userId, Message = message }; 
            Console.WriteLine($"Отправляем уведомление пользователю {notification.UserId}: {notification.Message}");

            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification); 
        }

    }
}
