using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;

namespace CourseDip.Application.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleService(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task<int> CreateModuleAsync(ModuleDto moduleDto)
        {
            var module = new Module
            {
                CourseId = moduleDto.CourseId,
                Title = moduleDto.Title,
                OrderIndex = moduleDto.OrderIndex
            };

            await _moduleRepository.AddAsync(module);
            return module.ModuleId;
        }

        public async Task<bool> DeleteModuleAsync(int id)
        {
            var module = await _moduleRepository.GetByIdAsync(id);
            if(module == null) return false;

            await _moduleRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ModuleDto>> GetModulesByCourseIdAsync(int id)
        {
            var modules = await _moduleRepository.GetModulesByCourseIdAsync(id);
            return modules.Select(m => new ModuleDto
            {
                ModuleId = m.ModuleId,
                CourseId = m.CourseId,
                Title = m.Title,
                OrderIndex = m.OrderIndex
            });
        }

        public async Task<ModuleDto?> GetModuleByIdAsync(int id)
        {
            var module = await _moduleRepository.GetByIdAsync(id);
            if (module == null) return null;

            return new ModuleDto
            {
                ModuleId = module.ModuleId,
                CourseId = module.CourseId,
                Title = module.Title,
                OrderIndex = module.OrderIndex,
                TestJson = module.TestJson,
            };

        }

        public async Task<bool> UpdateModuleAsync(int id, ModuleDto moduleDto)
        {
            var module = await _moduleRepository.GetByIdAsync(id);
            if (module == null) return false;

            module.Title = moduleDto.Title;
            module.OrderIndex = moduleDto.OrderIndex;
            module.TestJson = moduleDto.TestJson;

            await _moduleRepository.UpdateAsync(module);
            return true;
        }
    }
}
