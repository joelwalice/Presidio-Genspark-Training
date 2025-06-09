using System;
using Microsoft.AspNetCore.Mvc;
using JobPortalAPI.Contexts;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using JobPortalAPI.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

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
            var result = await _authService.LoginAsync(loginDto);
            // if (!result.Success)
            //     return Unauthorized(result.Message);

            return Ok(result);
        }

        [HttpPost("auth/refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRefreshDto refreshDto)
        {
            var result = await _authService.RefreshTokenAsync(refreshDto);
            // if (!result.Success)
            //     return Unauthorized(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("auth/logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (userId == null)
                return Unauthorized();

            await _authService.LogoutAsync(userId);
            return Ok(new { message = "Logged out successfully" });
        }

        [Authorize]
        [HttpGet("auth/me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (userId == null)
                return Unauthorized();

            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}