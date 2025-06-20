using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class FileDto
    {
        public int FileId { get; set; }
        public int UploaderId { get; set; }
        public int? LessonId { get; set; }
        public int? AssignmentId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }

    }
}
