using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class Submission
{
    public int SubmissionId { get; set; }

    public int AssignmentId { get; set; }

    public int UserId { get; set; }

    public DateTime SubmittedAt { get; set; }

    public string? Content { get; set; }

    public decimal? Grade { get; set; }

    public string? Feedback { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
    public virtual ICollection<File> Files { get; set; } = new List<File>();

}
