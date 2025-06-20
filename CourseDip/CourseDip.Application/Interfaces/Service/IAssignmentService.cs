using CourseDip.Application.DTO;

namespace CourseDip.Application.Interfaces.Service
{
    public interface IAssignmentService
    {
        Task<IEnumerable<AssignmentDto>> GetAssignmentsByLessonIdAsync(int lessonId);
        Task<AssignmentDto?> GetAssignmentByIdAsync(int id);
        Task<IEnumerable<AssignmentDto>> GetAssignmentsWithUpcomingDeadlinesAsync(int userId);
        Task<int> CreateAssignmentAsync(AssignmentDto assignmentDto);
        Task<bool> UpdateAssignmentAsync(int id, AssignmentDto assignmentDto);
        Task<bool> DeleteAssignmentAsync(int id);
    }
}
