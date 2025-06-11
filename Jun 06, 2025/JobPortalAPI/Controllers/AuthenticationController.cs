using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models.DTOs;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] UserAddRequestDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("auth/refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRefreshDto refreshDto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("auth/logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Invalid user identity." });

            try
            {
                await _authService.LogoutAsync(userId);
                return Ok(new { message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("auth/me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Invalid user identity." });

            try
            {
                var user = await _authService.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
