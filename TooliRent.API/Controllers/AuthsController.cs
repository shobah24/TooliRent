using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TooliRent.Application.Dto.User;
using TooliRent.Application.Services.Interfaces;
using TooliRent.Domain.Models;

namespace TooliRent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginDtoResponse>> Register(RegisterDto dto)
        {
            dto.Role = "Member";
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("registerAdmin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto dto)
        {
            dto.Role = "Admin";
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginDtoResponse>> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenRefreshResponseDto>> Refresh(TokenRefreshDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            return Ok(result);
        }
        [HttpPatch("Auth/{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeactivateUser(string id)
        {
            try
            {
                await _authService.DeActivateUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
        [HttpPatch("Auth/{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ActivateUser(string id)
        {
            try
            {
                await _authService.ActivateUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("revoke")]
        public async Task<ActionResult> Revoke(TokenRefreshDto dto)
        {
            var result = await _authService.RevokeTokenAsync(dto.RefreshToken);
            return result ? NoContent() : BadRequest(new { Message = "Token kunde inte återkallas." });
        }

        //[HttpGet("whoami")]
        //[Authorize]
        //public ActionResult WhoAmI()
        //{
        //    return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        //}
    }
}
