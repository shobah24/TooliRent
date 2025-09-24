namespace TooliRent.Application.Dto.Booking
{
    public class BookingReqDto
    {
        public int ToolId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
