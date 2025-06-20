using CourseDip.Application.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Service
{
    public interface ISubmissionService
    {
        Task<IEnumerable<SubmissionDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId);
        Task<IEnumerable<SubmissionDto>> GetSubmissionsByUserIdAsync(int userId);
        Task<SubmissionDto?> GetSubmissionByIdAsync(int id);
        Task<bool> GradeSubmissionAsync(int submissionId, int grade, string feedback);
        //Task<int> CreateSubmissionAsync(SubmissionDto submissionDto, List<IFormFile> files);
        Task<int> CreateSubmissionAsync(SubmissionDto submissionDto);
        Task<bool> UpdateSubmissionAsync(int id, SubmissionDto submissionDto);
        Task<bool> DeleteSubmissionAsync(int id);
    }
}
