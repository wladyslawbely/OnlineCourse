using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;

namespace CourseDip.Application.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly INotificationDispatcher _notificationDispatcher;

        public AssignmentService(IAssignmentRepository assignmentRepository, INotificationDispatcher notificationDispatcher, ILessonRepository lessonRepository)
        {
            _assignmentRepository = assignmentRepository;
            _notificationDispatcher = notificationDispatcher;
            _lessonRepository = lessonRepository;
        }

        public async Task<int> CreateAssignmentAsync(AssignmentDto assignmentDto)
        {
            var assignment = new Assignment
            {
                AssignmentId = assignmentDto.AssignmentId,
                LessonId = assignmentDto.LessonId,
                Description = assignmentDto.Description,
                Title = assignmentDto.Title,
                DueDate = assignmentDto.DueDate.HasValue
                        ? DateTime.SpecifyKind(assignmentDto.DueDate.Value, DateTimeKind.Utc) 
                        : null
            };

            var studentIds = await _lessonRepository.GetStudentIdsByLessonId(assignmentDto.LessonId);
            


            foreach (var studentId in studentIds)
            {
                Console.WriteLine(studentId.ToString());
                await _notificationDispatcher.SendNotificationAsync(studentId, $"Инструктор добавил новое задание: '{assignment.Title}'");
            }

            await _assignmentRepository.AddAsync(assignment);
            return assignment.AssignmentId;
        }

        public async Task<bool> DeleteAssignmentAsync(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null) return false;

            await _assignmentRepository.DeleteAsync(id);
            return true;
        }

        public async Task<AssignmentDto?> GetAssignmentByIdAsync(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null) return null;

            return new AssignmentDto
            {
                AssignmentId = assignment.AssignmentId,
                Description = assignment.Description,
                Title = assignment.Title,
                DueDate = assignment.DueDate,
            };
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentsByLessonIdAsync(int lessonId)
        {
            var assignments = await _assignmentRepository.GetByLessonIdAsync(lessonId);
            return assignments.Select(a => new AssignmentDto
            {
                AssignmentId = a.AssignmentId,
                LessonId = a.LessonId,
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate
            });
        }

        public async Task<bool> UpdateAssignmentAsync(int id, AssignmentDto assignmentDto)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null) return false;

            assignment.Title = assignmentDto.Title;
            assignment.Description = assignmentDto.Description;
            assignment.DueDate = assignmentDto.DueDate;

            await _assignmentRepository.UpdateAsync(assignment);
            return true;
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentsWithUpcomingDeadlinesAsync(int userId)
        {
            var upcomingAssignments = await _assignmentRepository.GetAssignmentsWithDeadlinesAsync(userId,DateTime.UtcNow.AddDays(3));

            return upcomingAssignments.Select(a => new AssignmentDto
            {
                AssignmentId = a.AssignmentId,
                LessonId = a.LessonId,
                Title = a.Title,
                Description = a.Description,
                DueDate = a.DueDate
            }).ToList();
        }
    }
}
