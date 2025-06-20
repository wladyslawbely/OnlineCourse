using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class SubmissionUpdateDto
    {
        public int SubmissionId { get; set; }

        public string? Content { get; set; }

        public List<IFormFile>? Files { get; set; }  
    }
}
