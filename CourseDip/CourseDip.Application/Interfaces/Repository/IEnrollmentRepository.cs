using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface IEnrollmentRepository
    {
        Task<IEnumerable<Enrollment>> GetEnrollmentsByUserIdAsync(int userId);
        Task<Enrollment?> GetEnrollmentAsync(int userId, int courseId);
        Task AddAsync(Enrollment enrollment);
        Task UpdateAsync(Enrollment enrollment);
        Task DeleteAsync(int enrollmentId);
    }
}
