using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Service
{
    public interface INotificationDispatcher
    {
        Task SendNotificationAsync(int userId, string message);
    }
}
