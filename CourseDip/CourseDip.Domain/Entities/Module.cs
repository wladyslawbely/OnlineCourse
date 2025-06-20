using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class Module
{
    public int ModuleId { get; set; }

    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public int OrderIndex { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public string? TestJson { get; set; }
}
