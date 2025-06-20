using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class File
{
    public int FileId { get; set; }

    public int UploaderId { get; set; }

    public int? LessonId { get; set; }

    public int? AssignmentId { get; set; }
    public int? SubmissionId { get; set; } 
    public virtual Submission? Submission { get; set; } 
    public string FileName { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public virtual Assignment? Assignment { get; set; }

    public virtual Lesson? Lesson { get; set; }

    public virtual User Uploader { get; set; } = null!;
}
