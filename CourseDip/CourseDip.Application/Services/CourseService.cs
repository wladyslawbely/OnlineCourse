using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;

namespace CourseDip.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseCategoryRepository _courseCategoryRepository;

        public CourseService(ICourseRepository courseRepository, ICourseCategoryRepository courseCategoryRepository )
        {
            _courseRepository = courseRepository;
            _courseCategoryRepository = courseCategoryRepository;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Description = c.Description ?? string.Empty,
                InstructorName = $"{c.Instructor.FirstName} {c.Instructor.LastName}",
                Categories = c.Categories.Select(cat => new CourseCategoryDto
                {
                    CategoryId = cat.CategoryId,
                    Name = cat.Name
                }).ToList()
            });
        }
        public async Task<IEnumerable<CourseDto>> GetCoursesByInstructorIdAsync(int instructorId)
        {
            var course = await _courseRepository.GetByUserIdAsync(instructorId);


            return course.Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Description = c.Description ?? string.Empty,
                Modules = c.Modules.Select(m => new ModuleDto
                {
                    ModuleId = m.ModuleId,
                    CourseId = m.CourseId,
                    Title = m.Title,
                    OrderIndex = m.OrderIndex,
                    TestJson = m.TestJson,
                    Lessons = m.Lessons.Select(lesson => new LessonDto
                    {
                        LessonId = lesson.LessonId,
                        ModuleId = lesson.ModuleId,
                        Title = lesson.Title,
                        Content = lesson.Content,
                        VideoUrl = lesson.VideoUrl,
                        OrderIndex = lesson.OrderIndex
                    }).ToList()
                }).ToList()
            });
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) return null;


            return new CourseDto
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description ?? string.Empty,
                InstructorName = $"{course.Instructor.FirstName} {course.Instructor.LastName}",
                Modules = course.Modules.Select(m => new ModuleDto
                {
                    ModuleId = m.ModuleId,
                    CourseId = m.CourseId,
                    Title = m.Title,
                    OrderIndex = m.OrderIndex,
                    TestJson = m.TestJson,
                    Lessons = m.Lessons.Select(lesson => new LessonDto
                    {
                        LessonId = lesson.LessonId,
                        ModuleId = lesson.ModuleId,
                        Title = lesson.Title,
                        Content = lesson.Content,
                        VideoUrl = lesson.VideoUrl,
                        OrderIndex = lesson.OrderIndex
                }).ToList()
                }).ToList(),
                Categories = course.Categories.Select(cat => new CourseCategoryDto
                {
                    CategoryId = cat.CategoryId,
                    Name = cat.Name
                }).ToList()
            };
        }


        public async Task<CourseDto> CreateCourseAsync(CourseDto courseDto, int instructorId)
        {
            var existingCategories = await _courseCategoryRepository
                .GetByIdsAsync(courseDto.Categories.Select(c => c.CategoryId));

            var course = new Course
            {
                Title = courseDto.Title,
                Description = courseDto.Description,
                InstructorId = courseDto.InstructorId,
                Categories = existingCategories.ToList()
            };

            await _courseRepository.AddAsync(course);

            return new CourseDto
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                InstructorId = course.InstructorId,
                Categories = course.Categories.Select(c => new CourseCategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                }).ToList()
            };
        }


        public async Task<bool> UpdateCourseAsync(int id, CourseDto courseDto)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) return false;

            course.Title = courseDto.Title;
            course.Description = courseDto.Description ?? string.Empty;
            course.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            course.InstructorId = courseDto.InstructorId;

            var categoryIds = courseDto.Categories.Select(c => c.CategoryId).ToList();

            var existingCategories = await _courseCategoryRepository.GetByIdsAsync(categoryIds);

            course.Categories.Clear();

            foreach (var cat in existingCategories)
            {
                course.Categories.Add(cat); 
            }

            await _courseRepository.UpdateAsync(course);
            return true;
        }


        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) return false;

            await _courseRepository.DeleteAsync(id);
            return true;
        }
    }
}
