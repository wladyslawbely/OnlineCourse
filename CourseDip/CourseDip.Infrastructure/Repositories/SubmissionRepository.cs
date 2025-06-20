using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using CourseDip.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Infrastructure.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {

        private readonly ApplicationDbContext _context;

        public SubmissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Submission submission)
        {
            _context.Submissions.Add(submission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var submission = await _context.Submissions.FindAsync(id);
            if (submission != null)
            {
                _context.Submissions.Remove(submission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Submission>> GetByAssignmentIdAsync(int assignmentId)
        {
            return await _context.Submissions
                .Where(s => s.AssignmentId == assignmentId).ToListAsync();
        }

        public async Task<Submission?> GetByIdAsync(int id)
        {
            return await _context.Submissions.FindAsync(id);
        }

        public async Task<IEnumerable<Submission>> GetByUserIdAsync(int userId)
        {
            return await _context.Submissions
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Submission submission)
        {
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateGradeAsync(int submissionId, int grade, string feedback)
        {
            var submission = await _context.Submissions.FindAsync(submissionId);
            if (submission == null) return false;

            submission.Grade = grade;
            submission.Feedback = feedback;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
