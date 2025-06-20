using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public DateTime? EnrolledAt { get; set; }

    public decimal? Progress { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public string? TestResultsJson { get; set; }
}
