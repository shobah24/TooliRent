namespace TooliRent.Application.Dto.Booking
{
    public class BookingReqDto
    {
        public IEnumerable<int> ToolId { get; set; } = new List<int>();
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
