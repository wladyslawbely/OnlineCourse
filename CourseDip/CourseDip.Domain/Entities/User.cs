using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Role { get; set; } = null!;

    public DateTime? RegisteredAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Lessonprogress> Lessonprogresses { get; set; } = new List<Lessonprogress>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
