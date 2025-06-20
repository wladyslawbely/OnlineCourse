using Microsoft.AspNetCore.SignalR;

namespace CourseDip.WebAPI.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(int userId, string message)
        {
            await Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("UserIdentifier пустой! Проверь аутентификацию!");
            }
            else
            {
                Console.WriteLine($"Подключился пользователь: {userId}");
            }

            await base.OnConnectedAsync();
        }
    }
}
