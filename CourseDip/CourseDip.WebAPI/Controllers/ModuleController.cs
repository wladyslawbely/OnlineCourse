using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourseDip.WebAPI.Controllers
{
    [Route("api/modules")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetModulesByCourse(int courseId)
        {
            var modules = await _moduleService.GetModulesByCourseIdAsync(courseId);
            return Ok(modules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModuleById(int id)
        {
            var module = await _moduleService.GetModuleByIdAsync(id);
            if (module == null) return NotFound($"Модуль с id {id} не найден.");
            return Ok(module);
        }

        [Authorize(Roles = "instructor,manager")]
        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] ModuleDto moduleDto)
        {
            var moduleId = await _moduleService.CreateModuleAsync(moduleDto);
            return CreatedAtAction(nameof(GetModuleById), new { id = moduleId }, moduleDto);
        }

        [Authorize(Roles = "instructor,manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(int id, [FromBody] ModuleDto moduleDto)
        {
            var updated = await _moduleService.UpdateModuleAsync(id, moduleDto);
            if (!updated) return NotFound($"Модуль с id {id} не найден.");
            return NoContent();
        }

        [Authorize(Roles = "instructor,manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var deleted = await _moduleService.DeleteModuleAsync(id);
            if (!deleted) return NotFound($"Модуль с id {id} не найден.");
            return NoContent();
        }

        [HttpGet("{moduleId}/test")]
        public async Task<IActionResult> GetTest(int moduleId)
        {
            var module = await _moduleService.GetModuleByIdAsync(moduleId);
            if (module == null || string.IsNullOrEmpty(module.TestJson))
                return NotFound("Тест не найден.");

            return Ok(JsonConvert.DeserializeObject<TestDto>(module.TestJson));
        }

        [Authorize(Roles = "instructor,manager")]
        [HttpPost("{moduleId}/test")]
        public async Task<IActionResult> SaveTest(int moduleId, [FromBody] TestDto testDto)
        {
            var module = await _moduleService.GetModuleByIdAsync(moduleId);
            if (module == null) return NotFound("Модуль не найден.");

            module.TestJson = JsonConvert.SerializeObject(testDto);
            await _moduleService.UpdateModuleAsync(moduleId, module);
            return Ok("Тест сохранён!");
        }

    }
}
