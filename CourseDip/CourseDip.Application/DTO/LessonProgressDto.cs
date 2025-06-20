using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class LessonProgressDto
    {
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    }
}
