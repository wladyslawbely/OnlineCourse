using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class TestDto
    {
        public string Title { get; set; } = string.Empty;
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }

    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public List<int> CorrectAnswers { get; set; } = new List<int>();

    }

}
