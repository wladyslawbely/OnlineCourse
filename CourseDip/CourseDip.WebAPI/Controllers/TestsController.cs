using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;

namespace CourseDip.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tests")]
    public class TestsController : ControllerBase
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        private readonly IEnrollmentService _enrollmentService;
        private readonly IModuleService _moduleService;

        public TestsController(IModuleRepository moduleRepository, IEnrollmentRepository enrollmentRepository, IEnrollmentService enrollmentService, IModuleService moduleService)
        {
            _moduleRepository = moduleRepository;
            _enrollmentRepository = enrollmentRepository;
            _enrollmentService = enrollmentService;
            _moduleService = moduleService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitTest([FromBody] TestSubmissionDto submission)
        {
            var module = await _moduleService.GetModuleByIdAsync(submission.ModuleId);
            if (module == null || string.IsNullOrEmpty(module.TestJson))
                return NotFound("Тест не найден.");

            var test = JsonConvert.DeserializeObject<TestDto>(module.TestJson);
            int correctCount = 0;

            foreach (var q in test.Questions)
            {
                var userAnswer = submission.Answers.FirstOrDefault(a => a.QuestionId == q.QuestionId);
                var correctAnswerTexts = q.CorrectAnswers.Select(idx => q.Options[idx]).ToList();
                if (userAnswer != null)
                {
                    var selectedAnswerTexts = userAnswer.SelectedAnswers.ToList();

                    Console.WriteLine($"Выбранные пользователем ответы (текст): {string.Join(", ", selectedAnswerTexts)}");

                    if (selectedAnswerTexts.SequenceEqual(correctAnswerTexts))
                    {
                        correctCount++;
                        Console.WriteLine("✅ Ответ верный!");
                    }
                }
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (!await _enrollmentService.IsUserEnrolledAsync(userId, module.CourseId))
                return NotFound("Запись на курс не найдена.");

            bool isUpdated = await _enrollmentService.UpdateTestResultsAsync(userId, submission.ModuleId, correctCount, submission.Answers.Count);
            return Ok(new { score = correctCount, total = test.Questions.Count });
        }

        [HttpPut("/api/modules/{moduleId}/test/{testId}")]
        [Authorize(Roles = "instructor,admin")]
        public async Task<IActionResult> UpdateTest(int moduleId, int testId, [FromBody] TestDto testDto)
        {
            var module = await _moduleRepository.GetByIdAsync(moduleId);
            if (module == null)
                return NotFound("Модуль не найден.");

            module.TestJson = JsonConvert.SerializeObject(testDto);

            await _moduleRepository.UpdateAsync(module);
            return Ok("Тест успешно обновлён.");
        }


    }
}
