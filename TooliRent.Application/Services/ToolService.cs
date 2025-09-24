using AutoMapper;
using TooliRent.Application.Dto.Tool;
using TooliRent.Application.Services.Interfaces;
using TooliRent.Domain.Models;
using TooliRent.Domain.RepoInterfaces;
using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Application.Services
{
    public class ToolService : IToolService
    {
        private readonly IToolRepository _repo;
        private readonly IMapper _mapper;

        public ToolService(IToolRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ToolDto>> GetAllToolAsync()
        {
            var tools = await _repo.GetAllToolAsync();
            return _mapper.Map<IEnumerable<ToolDto>>(tools);
          
        }

        public async Task<IEnumerable<ToolDto>> GetAvailableToolAsync()
        {
            var tools = await _repo.GetAvailableToolAsync();
            return _mapper.Map<IEnumerable<ToolDto>>(tools);
        }

        public async Task<IEnumerable<ToolDto>> GetToolByCategoryAsync(int categoryId)
        {
            var tools = await _repo.GetToolByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ToolDto>>(tools);
        }

        public async Task<ToolDto?> GetToolByIdAsync(int id)
        {
            var tool = await _repo.GetToolByIdAsync(id);
            return tool == null ? null : _mapper.Map<ToolDto>(tool);
        }

        public async Task<ToolDto> CreateToolAsync(CreateToolDto dto)
        {
            var tool = _mapper.Map<Tool>(dto);
            await _repo.AddToolAsync(tool);
            return _mapper.Map<ToolDto>(tool);
        }

        public async Task<ToolDto?> UpdateToolAsync(int id, UpdateToolDto dto)
        {
            var tool = await _repo.GetToolByIdAsync(id);
            if (tool == null) 
            { 
                throw new KeyNotFoundException($"Tool med ID:t {id} finns inte!");
            }
            _mapper.Map(dto, tool);
            await _repo.UpdateToolAsync(tool);

            //return _mapper.Map<ToolDto>(tool);
            var updatedTool = await _repo.GetToolByIdAsync(id);
            return _mapper.Map<ToolDto>(updatedTool);
        }

        public async Task<bool> DeleteToolAsync(int id)
        {
            var tool = await _repo.GetToolByIdAsync(id);
            if (tool is null)
            {
                //throw new KeyNotFoundException($"Tool med ID:t {id} finns inte!");
                return false;
            }

            await _repo.DeleteToolAsync(id);
            return true;
        }

        public async Task<IEnumerable<ToolDto>> FilterToolAsync(string status, int? categoryId)
        {
            if (!Enum.TryParse(status, true, out ToolStatus parsedStatus))
                throw new ArgumentException("Ogiltig status");

            var tools = await _repo.FilterToolAsync(parsedStatus, categoryId);
            return _mapper.Map<IEnumerable<ToolDto>>(tools);
        }
    }
}
