using CourseDip.Application.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Service
{
    public interface IFileService
    {
        Task<IEnumerable<FileDto>> GetFilesByLessonIdAsync(int lessonId);
        Task<IEnumerable<FileDto>> GetFilesByAssignmentIdAsync(int assignmentId);
        Task<FileDto?> GetFileByIdAsync(int id);
        Task<int> UploadFileAsync(FileUploadDto uploadDto, int userId);
        Task DeleteFileAsync(int id);
    }
}
