using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class FileUploadDto
    {
        public int UploaderId { get; set; }
        public IFormFile? File { get; set; } 
        public int LessonId { get; set; }
        public int? AssignmentId { get; set; }
    }
}
