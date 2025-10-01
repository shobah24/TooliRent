namespace TooliRent.Domain.Models
{
    public class BookingTool
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
        public int ToolId { get; set; }
        public Tool Tool { get; set; } = null!;
    }
}
