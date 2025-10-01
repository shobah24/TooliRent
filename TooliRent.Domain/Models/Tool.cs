using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Domain.Models
{
    public class Tool
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ToolStatus Status { get; set; } = ToolStatus.Available;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<BookingTool> BookingTools { get; set; } = new List<BookingTool>();

    }
}
