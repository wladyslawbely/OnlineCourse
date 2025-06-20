using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;

namespace CourseDip.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly string _uploadPath = "wwwroot/uploads";
        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task DeleteFileAsync(int id)
        {
            await _fileRepository.DeleteAsync(id);
        }

        public async Task<FileDto?> GetFileByIdAsync(int id)
        {
            var file = await _fileRepository.GetFileByIdAsync(id);
            if (file == null) return null;

            return new FileDto
            {
                UploaderId = file.UploaderId,
                LessonId = file.LessonId,
                AssignmentId = file.AssignmentId,
                FileName = file.FileName,
                FileUrl = file.FileUrl,
                UploadedAt = DateTime.SpecifyKind(file.UploadedAt, DateTimeKind.Unspecified)
            };
        }

        public async Task<IEnumerable<FileDto>> GetFilesByAssignmentIdAsync(int assignmentId)
        {
            var files = await _fileRepository.GetFilesByAssignmentIdAsync(assignmentId);
            return files.Select(f => new FileDto
            {
                UploaderId = f.UploaderId,
                LessonId = f.LessonId,
                AssignmentId = f.AssignmentId,
                FileName = f.FileName,
                FileUrl = f.FileUrl,
                UploadedAt = DateTime.SpecifyKind(f.UploadedAt, DateTimeKind.Unspecified)
            });
        }

        public async Task<IEnumerable<FileDto>> GetFilesByLessonIdAsync(int lessonId)
        {
            var files = await _fileRepository.GetFilesByLessonIdAsync(lessonId);
            return files.Select(f => new FileDto
            {
                UploaderId = f.UploaderId,
                LessonId = f.LessonId,
                AssignmentId = f.AssignmentId,
                FileName = f.FileName,
                FileUrl = f.FileUrl,
                UploadedAt = DateTime.SpecifyKind(f.UploadedAt, DateTimeKind.Unspecified)
            });
        }

        public async Task<int> UploadFileAsync(FileUploadDto uploadDto, int userId)
        {
            if (uploadDto.File == null || uploadDto.File.Length == 0)
                throw new ArgumentException("Файл отсутствует или пуст.");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            var filePath = Path.Combine(_uploadPath, uploadDto.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadDto.File.CopyToAsync(stream);
            }

            var fileEntity = new Domain.Entities.File
            {
                UploaderId = userId,
                LessonId = uploadDto.LessonId,
                AssignmentId = uploadDto.AssignmentId,
                FileName = uploadDto.File.FileName,
                FileUrl = $"/uploads/{uploadDto.File.FileName}",
                UploadedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            await _fileRepository.AddAsync(fileEntity);
            return fileEntity.FileId;
        }

    }
}
