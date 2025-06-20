using CourseDip.Application.Interfaces.Repository;
using CourseDip.Domain.Entities;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Application.DTO;
using Microsoft.AspNetCore.Http;
using File = CourseDip.Domain.Entities.File;

namespace CourseDip.Application.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IFileService _fileService;
        public SubmissionService(ISubmissionRepository submissionRepository,IUserRepository userRepository, IFileRepository fileRepository, IFileService fileService)
        {
            _submissionRepository = submissionRepository;
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _fileService = fileService;
        }

        public async Task<int> CreateSubmissionAsync(SubmissionDto submissionDto)
        {
            var submission = new Submission
            {
                AssignmentId = submissionDto.AssignmentId,
                UserId = submissionDto.UserId,
                SubmittedAt = DateTime.SpecifyKind(submissionDto.SubmittedAt, DateTimeKind.Unspecified),
                Content = submissionDto.Content,
                Grade = submissionDto.Grade,
                Feedback = submissionDto.Feedback
            };

            await _submissionRepository.AddAsync(submission);

            //foreach (var file in files)
            //{
            //    var filePath = Path.Combine("uploads", file.FileName);
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await file.CopyToAsync(stream);
            //    }

            //    var fileEntity = new File
            //    {
            //        FileName = file.FileName,
            //        FileUrl = "/uploads/" + file.FileName,
            //        SubmissionId = submission.SubmissionId 
            //    };

            //    await _fileRepository.AddAsync(fileEntity);
            //}
                
            return submission.SubmissionId;
            
        }

        public async Task<bool> DeleteSubmissionAsync(int id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission == null) return false;

            await _submissionRepository.DeleteAsync(id);

            return true;
        }

        public async Task<SubmissionDto?> GetSubmissionByIdAsync(int id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission == null) return null;

            return new SubmissionDto
            {
                SubmissionId = submission.SubmissionId,
                AssignmentId = submission.AssignmentId,
                UserId = submission.UserId,
                SubmittedAt = DateTime.SpecifyKind(submission.SubmittedAt, DateTimeKind.Unspecified),
                Content = submission.Content,
                Grade = submission.Grade,
                Feedback = submission.Feedback

            };
        }

        public async Task<IEnumerable<SubmissionDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId)
        {
            var submissions = await _submissionRepository.GetByAssignmentIdAsync(assignmentId);
            var userIds = submissions.Select(s => s.UserId).Distinct().ToList();
            var users = await _userRepository.GetUsersByIdsAsync(userIds);

            var userDict = users.ToDictionary(u => u.UserId);

            return submissions.Select(s => new SubmissionDto
            {
                SubmissionId = s.SubmissionId,
                AssignmentId = s.AssignmentId,
                UserId = s.UserId,
                UserName = userDict.ContainsKey(s.UserId)
                    ? userDict[s.UserId].FirstName + " " + userDict[s.UserId].LastName
                    : "Неизвестный пользователь",
                UserEmail = userDict.ContainsKey(s.UserId) ? userDict[s.UserId].Email : null,
                SubmittedAt = DateTime.SpecifyKind(s.SubmittedAt, DateTimeKind.Unspecified),
                Content = s.Content,
                Grade = s.Grade,
                Feedback = s.Feedback
            });
        }


        public async Task<bool> GradeSubmissionAsync(int submissionId, int grade, string feedback)
        {
            return await _submissionRepository.UpdateGradeAsync(submissionId, grade, feedback);
        }


        public async Task<IEnumerable<SubmissionDto>> GetSubmissionsByUserIdAsync(int userId)
        {
            var submissions = await _submissionRepository.GetByUserIdAsync(userId);
            return submissions.Select(s => new SubmissionDto
            {
                SubmissionId = s.SubmissionId,
                AssignmentId = s.AssignmentId,
                UserId = s.UserId,
                SubmittedAt = s.SubmittedAt,
                Content = s.Content,
                Grade = s.Grade,
                Feedback = s.Feedback,
                //Files = s.Files.Select(f => new FileDto
                //{
                //    FileId = f.FileId,
                //    FileName = f.FileName,
                //    FileUrl = f.FileUrl
                //}).ToList()
           
            });
        }

      

        public async Task<bool> UpdateSubmissionAsync(int id,SubmissionDto submissionDto)
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission == null) return false;

            submission.Content = submissionDto.Content;
            submission.Grade = submissionDto.Grade;
            submission.Feedback = submissionDto.Feedback;

            await _submissionRepository.UpdateAsync(submission);
            return true;
        }
    }
}
