using CourseDip.Application.DTO;

namespace CourseDip.Application.Interfaces.Service
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetCommentsByLessonIdAsync(int lessonId);
        Task<int> CreateCommentAsync(CommentDto commentDto, int userId);
        Task<bool> UpdateCommentAsync(int id, CommentDto commentDto);
        Task<bool> DeleteCommentAsync(int id);
        Task<IEnumerable<CommentDto>> GetCommentsByCourseIdAsync(int courseId);
    }
}
