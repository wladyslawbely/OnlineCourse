using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class DeadlineNotificationDto
    {
        public int UserId { get; set; }
        public string? AssignmentTitle { get; set; } 
        public DateTime DueDate { get; set; } 
        public string? Message { get; set; }
    }
}
