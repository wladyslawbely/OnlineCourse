using CourseDip.Application.DTO;

namespace CourseDip.Application.Interfaces.Service
{
    public interface ICourseCategoryService
    {

        Task<IEnumerable<CourseCategoryDto>> GetAllCategoriesAsync();
        Task<CourseCategoryDto?> GetCategoryByIdAsync(int id);
        Task<int> CreateCategoryAsync(CourseCategoryDto courseCategoryDto);
        Task<bool> UpdateCategoryAsync(int id, CourseCategoryDto courseCategoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
