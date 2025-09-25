using TooliRent.Application.Dto.Category;

namespace TooliRent.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryWithoutToolsDtos>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto?> GetCategoryWithToolsAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
