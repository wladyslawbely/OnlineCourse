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
    public class LessonService : ILessonService
    {

        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<int> CreateLessonAsync(LessonDto lessonDto)
        {
            var lesson = new Lesson
            {
                ModuleId = lessonDto.ModuleId,
                Title = lessonDto.Title,
                Content = lessonDto.Content,
                VideoUrl = lessonDto.VideoUrl,
                OrderIndex = lessonDto.OrderIndex
            };

            await _lessonRepository.AddAsync(lesson);
            return lesson.LessonId;

        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null) return false;

            await _lessonRepository.DeleteAsync(id);
            return true;
        }

        public async Task<LessonDto?> GetLessonByIdAsync(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null) return null;

            return new LessonDto
            {
                LessonId = lesson.LessonId,
                ModuleId = lesson.ModuleId,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                OrderIndex = lesson.OrderIndex
            };
        }

        public async Task<IEnumerable<LessonDto>> GetLessonsByModuleIdAsync(int moduleId)
        {
            var lessons = await _lessonRepository.GetLessonByModuleIdAsync(moduleId);
            return lessons.Select(l => new LessonDto
            {
                LessonId = l.LessonId,
                ModuleId = l.ModuleId,
                Title = l.Title,
                Content = l.Content,
                VideoUrl = l.VideoUrl,
                OrderIndex = l.OrderIndex
            });
        }

        public async Task<bool> UpdateLessonAsync(int id, LessonDto lessonDto)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null) return false;

            lesson.Title = lessonDto.Title;
            lesson.Content = lessonDto.Content;
            lesson.VideoUrl = lessonDto.VideoUrl;
            lesson.OrderIndex = lessonDto.OrderIndex;

            await _lessonRepository.UpdateAsync(lesson);
            return true;
        }
    }
}
