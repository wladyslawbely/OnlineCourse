using CourseDip.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Service
{
    public interface ILessonProgressService
    {
        Task<bool> TrackLessonProgressAsync(LessonProgressDto progressDto);
        Task<List<int>> GetCompletedLessonIdsByUserAsync(int userId);
    }
}
