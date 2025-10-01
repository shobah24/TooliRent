using TooliRent.Domain.Enums;
using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        //public int ToolId { get; set; }
        //public Tool Tool { get; set; } = null!;
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }

        // förseningar.....
        public DateTime PickupDate { get; set; }
        public DateTime ReturnedDate { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public ICollection<BookingTool> BookingTools { get; set; } = new List<BookingTool>();

    }
}
