using CourseDip.Application.DTO;
using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CourseDip.Application.Services
{
    public class CourseCategoryService : ICourseCategoryService
    {
        private readonly ICourseCategoryRepository _courseCategoryRepository;
        public CourseCategoryService(ICourseCategoryRepository courseCategoryRepository)
        {
            _courseCategoryRepository = courseCategoryRepository;
        }

        public async Task<int> CreateCategoryAsync(CourseCategoryDto courseCategoryDto)
        {
            var category = new Coursecategory { Name = courseCategoryDto.Name };
            await _courseCategoryRepository.AddAsync(category);
            return category.CategoryId;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _courseCategoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            await _courseCategoryRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<CourseCategoryDto>> GetAllCategoriesAsync()
        { 
            var category = await _courseCategoryRepository.GetAllAsync();
            return category.Select(c => new CourseCategoryDto
            {
                Name = c.Name,
                CategoryId = c.CategoryId
            });
        }

        public async Task<CourseCategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _courseCategoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return new CourseCategoryDto
            {
                Name = category.Name,
                CategoryId = category.CategoryId              
            };
        }

        public async Task<bool> UpdateCategoryAsync(int id, CourseCategoryDto courseCategoryDto)
        {
            var category = await _courseCategoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            category.Name = courseCategoryDto.Name;
            await _courseCategoryRepository.UpdateAsync(category);
            return true;
        }
    }
}
