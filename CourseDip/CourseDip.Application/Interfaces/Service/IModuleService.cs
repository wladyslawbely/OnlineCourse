using CourseDip.Application.DTO;
using CourseDip.Domain.Entities;

namespace CourseDip.Application.Interfaces.Service
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleDto>> GetModulesByCourseIdAsync(int id);
        Task<ModuleDto?> GetModuleByIdAsync(int id);
        Task<int> CreateModuleAsync(ModuleDto moduleDto);
        Task<bool> UpdateModuleAsync(int id, ModuleDto moduleDto);
        Task<bool> DeleteModuleAsync(int id);
    }
}
