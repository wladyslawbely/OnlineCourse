using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDip.Application.Interfaces.Repository
{
    public interface ICourseCategoryRepository
    {
        Task<IEnumerable<Coursecategory>> GetAllAsync();
        Task<Coursecategory?> GetByIdAsync(int id);
        Task<IEnumerable<Coursecategory>> GetByIdsAsync(IEnumerable<int> ids);
        Task AddAsync(Coursecategory category);
        Task UpdateAsync(Coursecategory category);
        Task DeleteAsync(int id);
    }
}
