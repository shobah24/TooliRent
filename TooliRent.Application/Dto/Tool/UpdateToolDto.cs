using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Application.Dto.Tool
{
    public class UpdateToolDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ToolStatus Status { get; set; }
    }
}
