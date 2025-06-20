using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.DTO
{
    public class TestSubmissionDto
    {
        public int UserId { get; set; } 
        public int ModuleId { get; set; } 
        public List<UserAnswerDto> Answers { get; set; } 
    }

    public class UserAnswerDto
    {
        public int QuestionId { get; set; }
        public List<string> SelectedAnswers { get; set; } 
    }
}
