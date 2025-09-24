using TooliRent.Domain.Models;
using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Domain.RepoInterfaces
{
    public interface IToolRepository
    {
        Task<IEnumerable<Tool>> GetAllToolAsync();
        Task<Tool?> GetToolByIdAsync(int id);
        Task<IEnumerable<Tool>> GetToolByCategoryAsync(int categoryId);
        Task<IEnumerable<Tool>> GetAvailableToolAsync();
        Task AddToolAsync(Tool tool);
        Task UpdateToolAsync(Tool tool);
        Task DeleteToolAsync(int id);

        Task<IEnumerable<Tool>> FilterToolAsync(ToolStatus status, int? categoryId);
    }
}
