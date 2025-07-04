using System.Security.Claims;
using JobPortalAPI.Models.DTOs;
using JobPortalAPI.Models;


namespace JobPortalAPI.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResultDto> LoginAsync(UserAddRequestDto loginDto);
        Task<AuthResultDto> RefreshTokenAsync(TokenRefreshDto refreshDto);
        Task LogoutAsync(string userId);
        Task<UserAddRequestDto?> GetUserByIdAsync(string userId);
    }
}