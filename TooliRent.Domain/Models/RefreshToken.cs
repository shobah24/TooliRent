﻿namespace TooliRent.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; } = false;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}