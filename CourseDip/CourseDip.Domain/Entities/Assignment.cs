using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public int LessonId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
