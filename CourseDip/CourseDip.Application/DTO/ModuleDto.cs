using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class ModuleDto
    {
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public string TestJson { get; set; } = string.Empty;
        public List<LessonDto> Lessons { get; set; } = new();
    }
}
