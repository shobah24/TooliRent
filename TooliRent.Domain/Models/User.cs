using Microsoft.AspNetCore.Identity;


namespace TooliRent.Domain.Models
{
    public class User : IdentityUser
    {

        //public string? RefreshToken { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
