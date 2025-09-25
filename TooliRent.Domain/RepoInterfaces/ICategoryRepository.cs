using TooliRent.Domain.Models;

namespace TooliRent.Domain.RepoInterfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        //Task DeleteCategoryAsync(int id);
        Task DeleteCategoryAsync(Category category);
        Task<Category?> GetCategoryWithToolsAsync(int id);
    }
}
