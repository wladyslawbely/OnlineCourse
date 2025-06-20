using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int? CourseId { get; set; }
        public int? LessonId { get; set; }
        public int? ParentCommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentDto> Replies { get; set; } = new();
    }
}
