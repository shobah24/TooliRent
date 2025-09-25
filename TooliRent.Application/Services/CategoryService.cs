using AutoMapper;
using TooliRent.Application.Dto.Category;
using TooliRent.Application.Services.Interfaces;
using TooliRent.Domain.Models;
using TooliRent.Domain.RepoInterfaces;

namespace TooliRent.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryWithoutToolsDtos>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryWithoutToolsDtos>>(categories);

        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto?>(category);

        }

        public async Task<CategoryDto?> GetCategoryWithToolsAsync(int id)
        {
            var category = await _repo.GetCategoryWithToolsAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);


        }
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _repo.AddCategoryAsync(category);
            return _mapper.Map<CategoryDto>(category);
        }
        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category med ID:t {id} finns inte!");
            }
            _mapper.Map(dto, category);
            await _repo.UpdateCategoryAsync(category);
            var updatedCategory = await _repo.GetCategoryByIdAsync(id);
            return _mapper.Map<CategoryDto?>(updatedCategory);


        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return false;
            }
            await _repo.DeleteCategoryAsync(category);
            return true;
        }

    }
}
