using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int InstructorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public List<ModuleDto> Modules { get; set; } = new();
        public List<CourseCategoryDto> Categories { get; set; } = new();
    }
}
