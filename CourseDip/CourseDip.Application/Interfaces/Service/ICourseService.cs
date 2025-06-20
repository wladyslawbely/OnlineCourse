using CourseDip.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Service
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(int id);
        Task<IEnumerable<CourseDto>> GetCoursesByInstructorIdAsync(int instructorId);
        Task<CourseDto> CreateCourseAsync(CourseDto courseDto, int instructorId);
        Task<bool> UpdateCourseAsync(int id, CourseDto courseDto);
        Task<bool> DeleteCourseAsync(int id);

    }
}
