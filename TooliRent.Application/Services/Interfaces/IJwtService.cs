using TooliRent.Domain.Models;

namespace TooliRent.Application.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(User user);
        Task<string> GenerateRefreshToken();
    }
}
