using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class SubmissionDto
    {
        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string? Content { get; set; } = string.Empty;
        public decimal? Grade { get; set; }
        public string? Feedback { get; set; }
        //public List<FileDto> Files { get; set; } = new();
    }
}
