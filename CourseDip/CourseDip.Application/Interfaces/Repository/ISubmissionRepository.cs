using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface ISubmissionRepository
    {
        Task<IEnumerable<Submission>> GetByAssignmentIdAsync(int assignmentId);
        Task<IEnumerable<Submission>> GetByUserIdAsync(int userId);
        Task<Submission?> GetByIdAsync(int id);
        Task<bool> UpdateGradeAsync(int submissionId, int grade, string feedback);
        Task AddAsync(Submission submission);
        Task UpdateAsync(Submission submission);
        Task DeleteAsync(int id);
    }
}
