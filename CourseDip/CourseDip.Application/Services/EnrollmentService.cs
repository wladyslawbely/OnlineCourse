using CourseDip.Application.Interfaces.Service;
using CourseDip.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using Newtonsoft.Json;

namespace CourseDip.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILessonProgressRepository _lessonProgressRepository;
        private readonly IModuleRepository _moduleRepository;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepository,
            ICourseRepository courseRepository,
            IUserRepository userRepository,
            ILessonRepository lessonRepository,
            ILessonProgressRepository lessonProgressRepository,
            IModuleRepository moduleRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _lessonRepository = lessonRepository;
            _lessonProgressRepository = lessonProgressRepository;
            _moduleRepository = moduleRepository;
        }

        public async Task<bool> EnrollStudentAsync(EnrollmentDto enrollmentDto)
        {
            var course = await _courseRepository.GetByIdAsync(enrollmentDto.CourseId);
            if (course == null) return false;

            var user = await _userRepository.GetByIdAsync(enrollmentDto.UserId);
            if (user == null) return false;

            var existingEnrollment = await _enrollmentRepository.GetEnrollmentAsync(enrollmentDto.UserId, enrollmentDto.CourseId);
            if (existingEnrollment != null) return false;

            var enrollment = new Enrollment
            {
                UserId = enrollmentDto.UserId,
                CourseId = enrollmentDto.CourseId,
                EnrolledAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                Progress = enrollmentDto.Progress
            };

            await _enrollmentRepository.AddAsync(enrollment);
            return true;
        }

        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByUserIdAsync(int userId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByUserIdAsync(userId);
            var enrollmentDtos = new List<EnrollmentDto>();

            foreach (var e in enrollments)
            {
                var calculatedProgress = await CalculateCourseProgressAsync(e.UserId, e.CourseId);

                enrollmentDtos.Add(new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    UserId = e.UserId,
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.Title,
                    EnrolledAt = e.EnrolledAt,
                    Progress = calculatedProgress 
                });
            }
            return enrollmentDtos;
        }


        public async Task<bool> UnenrollStudentAsync(EnrollmentDto enrollmentDto)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentAsync(enrollmentDto.UserId, enrollmentDto.CourseId);
            if (enrollment == null) return false;

            await _enrollmentRepository.DeleteAsync(enrollment.EnrollmentId);
            return true;

        }

        public async Task<bool> UpdateProgressAsync(int userId, int courseId, decimal progress)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentAsync(userId, courseId);
            if (enrollment == null) return false;

            enrollment.Progress = Math.Clamp(progress, 0, 100);
            await _enrollmentRepository.UpdateAsync(enrollment);
            return true;
        }

        public async Task<bool> UpdateTestResultsAsync(int userId, int moduleId, int correctCount, int totalQuestions)
        {
            var module = await _moduleRepository.GetByIdAsync(moduleId);
            if (module == null)
                throw new Exception("Модуль не найден.");

            var enrollment = await _enrollmentRepository.GetEnrollmentAsync(userId, module.CourseId);
            if (enrollment == null)
                throw new Exception("Запись на курс не найдена.");

            enrollment.TestResultsJson = JsonConvert.SerializeObject(new
            {
                score = correctCount,
                total = totalQuestions
            });

            await _enrollmentRepository.UpdateAsync(enrollment);

            return true;    
        }

        public async Task<bool> IsUserEnrolledAsync(int userId, int courseId)
        {
            return await _enrollmentRepository.GetEnrollmentAsync(userId, courseId) != null;
        }

        public async Task<decimal> CalculateCourseProgressAsync(int userId, int courseId)
        {
            var totalLessons = await _lessonRepository.CountLessonsInCourseAsync(courseId);
            if (totalLessons == 0) return 0;

            var completedLessons = await _lessonProgressRepository.CountCompletedLessonsAsync(userId, courseId);
            var progress = (decimal)completedLessons / totalLessons * 100;
            return Math.Round(progress, 2);
        }
    }
}
