using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class Comment
{
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int? LessonId { get; set; }
    public int? CourseId { get; set; }
    public int? ParentCommentId { get; set;}
    public string Content { get; set; } = null!;
    public bool IsAdmin { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Lesson Lesson { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Course? Course { get; set; }
    public virtual Comment? ParentComment { get; set; } 
    public virtual List<Comment> Replies { get; set; } = new();
}
