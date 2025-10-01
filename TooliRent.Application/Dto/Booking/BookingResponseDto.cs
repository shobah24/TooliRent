using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Application.Dto.Booking
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        //public int ToolId { get; set; }
        public IEnumerable<string> ToolName { get; set; } = new List<string>();
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? PickupDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }
}
