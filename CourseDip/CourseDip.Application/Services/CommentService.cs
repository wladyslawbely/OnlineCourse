using CourseDip.Application.Interfaces.Service;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.DTO;
using CourseDip.Domain.Entities;

namespace CourseDip.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<int> CreateCommentAsync(CommentDto commentDto, int userId)
        {
            var comment = new Comment
            {
                UserId = userId,
                LessonId = commentDto.LessonId,
                CourseId = commentDto.CourseId, 
                ParentCommentId = commentDto.ParentCommentId, 
                Content = commentDto.Content,
                IsAdmin = commentDto.IsAdmin, 
                CreatedAt = DateTime.UtcNow

            };

            await _commentRepository.AddAsync(comment);
            return comment.CommentId;
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return false;

            var replies = await _commentRepository.GetRepliesAsync(id);
            foreach (var reply in replies)
            {
                await _commentRepository.DeleteAsync(reply.CommentId);
            }

            await _commentRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByLessonIdAsync(int lessonId)
        {
            var comments = await _commentRepository.GetByLessonIdAsync(lessonId);
            return comments.Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                UserId = c.UserId,
                LessonId = c.LessonId,
                Content = c.Content,
                IsAdmin = c.IsAdmin, 
                CreatedAt = DateTime.UtcNow,
                Replies = c.Replies.Select(r => new CommentDto
                {
                    CommentId = r.CommentId,
                    UserId = r.UserId,
                    Content = r.Content,
                    IsAdmin = r.IsAdmin, 
                    CreatedAt = DateTime.UtcNow,
                    ParentCommentId = r.ParentCommentId
                }).ToList()
            });
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByCourseIdAsync(int courseId)
        {
            var comments = await _commentRepository.GetByCourseIdAsync(courseId);

            var parentComments = comments.Where(c => c.ParentCommentId == null).Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                UserId = c.UserId,
                UserName = c.User?.FirstName ?? "Аноним",
                CourseId = c.CourseId,
                Content = c.Content,
                IsAdmin = c.IsAdmin,
                CreatedAt = c.CreatedAt,

                Replies = comments.Where(r => r.ParentCommentId == c.CommentId).Select(r => new CommentDto
                {
                    CommentId = r.CommentId,
                    UserId = r.UserId,
                    UserName = r.User?.FirstName ?? "Аноним",
                    Content = r.Content,
                    IsAdmin = r.IsAdmin,
                    CreatedAt = r.CreatedAt,
                    ParentCommentId = r.ParentCommentId
                }).ToList()
            }).ToList();

            return parentComments;
        }

        public async Task<bool> UpdateCommentAsync(int id, CommentDto commentDto)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return false;

            comment.Content = commentDto.Content;
            comment.IsAdmin = commentDto.IsAdmin;

            await _commentRepository.UpdateAsync(comment);
            return true;
        }
    }
}
