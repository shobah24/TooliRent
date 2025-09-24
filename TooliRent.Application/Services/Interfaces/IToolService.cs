using TooliRent.Application.Dto.Tool;

namespace TooliRent.Application.Services.Interfaces
{
    public interface IToolService
    {
        Task<IEnumerable<ToolDto>> GetAllToolAsync();
        Task<ToolDto?> GetToolByIdAsync(int id);
        Task<IEnumerable<ToolDto>> GetToolByCategoryAsync(int categoryId);
        Task<IEnumerable<ToolDto>> GetAvailableToolAsync();
        Task<ToolDto> CreateToolAsync(CreateToolDto dto);
        Task<ToolDto?> UpdateToolAsync(int id, UpdateToolDto dto);
        Task<bool> DeleteToolAsync(int id);
        Task<IEnumerable<ToolDto>> FilterToolAsync(string status, int? categoryId);
    }
}
