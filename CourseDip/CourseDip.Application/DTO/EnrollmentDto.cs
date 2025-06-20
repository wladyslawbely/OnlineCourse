using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public DateTime? EnrolledAt { get; set; } = DateTime.UtcNow;
        public decimal? Progress { get; set; } = 0;
    }
}