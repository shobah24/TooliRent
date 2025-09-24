using Microsoft.AspNetCore.Identity;


namespace TooliRent.Domain.Models
{
    public class User : IdentityUser
    {
       
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
