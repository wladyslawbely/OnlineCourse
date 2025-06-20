using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                var replies = await _context.Comments.Where(c => c.ParentCommentId == commentId).ToListAsync();
                _context.Comments.RemoveRange(replies);

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Comment>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Comments
                .Where(c => c.LessonId == lessonId && c.CourseId == null)
                .Include(c => c.Replies) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Comments
                .Where(c => c.CourseId == courseId && c.LessonId == null)
                .Include(c => c.User)
                .Include(c => c.Replies) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(c => c.Replies).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<IEnumerable<Comment>> GetRepliesAsync(int parentCommentId)
        {
            return await _context.Comments
                .Where(c => c.ParentCommentId == parentCommentId)
                .ToListAsync();
        }
    }

}
