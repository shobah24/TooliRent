namespace TooliRent.Application.Dto.Booking
{
    public class BookingStatsDto
    {
        public int Total { get; set; }
        public int Pending { get; set; }
        public int Active { get; set; }
        public int Returned { get; set; }
        public int Late { get; set; }
    }
}
