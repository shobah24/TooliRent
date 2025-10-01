using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TooliRent.Application.Dto.User;
using TooliRent.Application.Services.Interfaces;
using TooliRent.Domain.Models;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly TooliRentDbContext _context;
        public AuthService(UserManager<User> userManager, IJwtService jwtService, TooliRentDbContext context)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _context = context;
        }
        public async Task<LoginDtoResponse> RegisterAsync(RegisterDto dto)
        {
            var user = new User { UserName = dto.UserName, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var role = string.IsNullOrWhiteSpace(dto.Role) ? "Member" : dto.Role;  // kkkk
            await _userManager.AddToRoleAsync(user, role);

            var token = await _jwtService.GenerateJwtToken(user);
            var refreshToken = await _jwtService.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, Expires = DateTime.Now.AddDays(7), User = user });
            await _context.SaveChangesAsync();

            return new LoginDtoResponse
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Token = token,
                RefreshToken = refreshToken
            };
        }
        public async Task<LoginDtoResponse> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                throw new Exception("Felaktigt användarnamn eller lösenord.");
            }

            var token = await _jwtService.GenerateJwtToken(user);
            var refreshToken = await _jwtService.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, Expires = DateTime.Now.AddDays(7), User = user });
            await _context.SaveChangesAsync();

            return new LoginDtoResponse
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Token = token,
                RefreshToken = refreshToken
            };
        }
        public async Task<TokenRefreshResponseDto> RefreshTokenAsync(TokenRefreshDto dto)
        {
            var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken && !rt.IsRevoked);

            if (refreshToken == null || refreshToken.Expires < DateTime.Now)
            {
                throw new Exception("Ogiltig eller utgången refresh token.");
            }

            var newAccessToken = await _jwtService.GenerateJwtToken(refreshToken.User);
            var newRefreshToken = await _jwtService.GenerateRefreshToken();

            refreshToken.IsRevoked = true;
            refreshToken.User.RefreshTokens.Add(new RefreshToken { Token = newRefreshToken, Expires = DateTime.Now.AddDays(7), User = refreshToken.User });

            await _context.SaveChangesAsync();

            return new TokenRefreshResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.Now.AddMinutes(60)
            };
        }


        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (token is null)
            {
                return false;
            }

            token.IsRevoked = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
