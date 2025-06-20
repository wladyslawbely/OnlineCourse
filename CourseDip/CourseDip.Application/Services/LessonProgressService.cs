using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Services
{
    public class LessonProgressService : ILessonProgressService
    {
        private readonly ILessonProgressRepository _lessonProgressRepository;
        public LessonProgressService(ILessonProgressRepository lessonProgressRepository)
        {
            _lessonProgressRepository = lessonProgressRepository;
        }

        public async Task<bool> TrackLessonProgressAsync(LessonProgressDto progressDto)
        {
            var existingProgress = await _lessonProgressRepository.GetByUserAndLessonAsync(progressDto.UserId, progressDto.LessonId);

            if (existingProgress != null)
            {
                existingProgress.CompletedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                await _lessonProgressRepository.UpdateAsync(existingProgress);
                return false; 
            }

            var newProgress = new Lessonprogress
            {
                UserId = progressDto.UserId,
                LessonId = progressDto.LessonId,
                CompletedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
            };

            await _lessonProgressRepository.AddAsync(newProgress);
            return true; 
        }

        public async Task<List<int>> GetCompletedLessonIdsByUserAsync(int userId)
        {
            var progressList = await _lessonProgressRepository.GetByUserIdAsync(userId);
            return progressList.Select(p => p.LessonId).ToList();
        }

    }
}
