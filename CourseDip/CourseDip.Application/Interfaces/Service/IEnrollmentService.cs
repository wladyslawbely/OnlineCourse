using CourseDip.Application.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Service
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByUserIdAsync(int userId);
        Task<bool> EnrollStudentAsync(EnrollmentDto enrollmentDto);
        Task<bool> UnenrollStudentAsync(EnrollmentDto enrollmentDto);
        Task<bool> UpdateProgressAsync(int userId, int courseId, decimal progress);
        Task<bool> IsUserEnrolledAsync(int userId, int courseId);
        Task<decimal> CalculateCourseProgressAsync(int userId, int courseId);
        Task<bool> UpdateTestResultsAsync(int userId, int moduleId, int correctCount, int totalQuestions);

    }
}
