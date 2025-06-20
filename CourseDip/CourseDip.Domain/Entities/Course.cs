using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int InstructorId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual User Instructor { get; set; } = null!;

    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();

    public virtual ICollection<Coursecategory> Categories { get; set; } = new List<Coursecategory>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
