using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Application.Dto.Booking
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        //public int ToolId { get; set; }
        public string ToolName { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
