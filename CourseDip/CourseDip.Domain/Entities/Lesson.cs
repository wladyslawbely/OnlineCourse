using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class Lesson
{
    public int LessonId { get; set; }

    public int ModuleId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public string? VideoUrl { get; set; }

    public int OrderIndex { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Lessonprogress> Lessonprogresses { get; set; } = new List<Lessonprogress>();

    public virtual Module Module { get; set; } = null!;
}
