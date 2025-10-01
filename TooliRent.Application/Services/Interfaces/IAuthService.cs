using TooliRent.Application.Dto.User;

namespace TooliRent.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginDtoResponse> LoginAsync(LoginDto dto);
        Task<LoginDtoResponse> RegisterAsync(RegisterDto dto);
        Task<TokenRefreshResponseDto> RefreshTokenAsync(TokenRefreshDto dto);
        Task<bool> RevokeTokenAsync(string refreshToken);

        Task DeActivateUserAsync(string userId);
        Task ActivateUserAsync(string userId);
    }
}
