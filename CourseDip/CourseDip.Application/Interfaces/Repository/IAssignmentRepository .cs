using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface IAssignmentRepository
    {
        Task<IEnumerable<Assignment>> GetByLessonIdAsync(int lessonId);
        Task<Assignment?> GetByIdAsync(int id);
        Task<IEnumerable<Assignment>> GetAssignmentsWithDeadlinesAsync(int userId, DateTime deadlineTime);
        Task AddAsync(Assignment assignment);
        Task UpdateAsync(Assignment assignment);
        Task DeleteAsync(int id);

    }
}
