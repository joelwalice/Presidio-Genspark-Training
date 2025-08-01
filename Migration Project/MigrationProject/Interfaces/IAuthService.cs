using MigrationProject.DTOs;
using MigrationProject.Models;

public interface IAuthService
{
    Task<bool> RegisterAsync(UserRegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(UserLoginDto dto);
}