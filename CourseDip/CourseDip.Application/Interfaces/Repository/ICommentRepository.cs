using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetByLessonIdAsync(int lessonId);
        Task<IEnumerable<Comment>> GetByCourseIdAsync(int courseId);
        Task<Comment?> GetByIdAsync(int id);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task<IEnumerable<Comment>> GetRepliesAsync(int parentCommentId);
        Task DeleteAsync(int commentId);
    }
}
