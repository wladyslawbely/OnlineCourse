using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllAsync();
        Task<Module?> GetByIdAsync(int id);
        Task<IEnumerable<Module>> GetModulesByCourseIdAsync(int id);
        Task AddAsync(Module module);
        Task DeleteAsync(int id);
        Task UpdateAsync(Module module);
    }
}
