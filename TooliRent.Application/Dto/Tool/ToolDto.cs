using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Application.Dto.Tool
{
    public class ToolDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public ToolStatus Status { get; set; }
    }
}
