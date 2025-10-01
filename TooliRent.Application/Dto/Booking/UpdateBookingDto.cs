using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Application.Dto.Booking
{
    public class UpdateBookingDto
    {
        public IEnumerable<int>? ToolId { get; set; }
        //public string ToolName { get; set; } = string.Empty;
        public DateTime? LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BookingStatus? Status { get; set; }
    }
}
