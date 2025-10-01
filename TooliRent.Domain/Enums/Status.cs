namespace TooliRent.Domain.Enums
{
    public class Status
    {
        public enum ToolStatus
        {
            Available,
            Rented,
            Maintenance,
            Damaged

        }
        public enum BookingStatus
        {
            Pending,
            Booked,
            Loaned,
            Active,
            Returned,
            Cancelled,
            Late
        }
    }
}
