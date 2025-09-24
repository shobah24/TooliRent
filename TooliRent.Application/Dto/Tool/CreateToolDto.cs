using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Application.Dto.Tool
{
    public class CreateToolDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
        public ToolStatus Status { get; set; } = ToolStatus.Available;
    }
}
