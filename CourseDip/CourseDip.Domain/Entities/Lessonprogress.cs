using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class Lessonprogress
{
    public int ProgressId { get; set; }

    public int UserId { get; set; }

    public int LessonId { get; set; }

    public DateTime CompletedAt { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
