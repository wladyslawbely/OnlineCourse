using System;
using System.Collections.Generic;

namespace CourseDip.Domain.Entities;

public partial class Coursecategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
