using TooliRent.Application.Dto.Tool;

namespace TooliRent.Application.Dto.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public IEnumerable<ToolDto> Tools { get; set; } = new List<ToolDto>();
    }
}
