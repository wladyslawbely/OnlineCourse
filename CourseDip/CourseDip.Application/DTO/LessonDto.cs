using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class LessonDto
    {
        public int LessonId { get; set; }
        public int ModuleId { get; set; }
        public string? Title { get; set; } 
        public string? Content { get; set; }
        public string? VideoUrl { get; set; }
        public int OrderIndex { get; set; }
    }
}
