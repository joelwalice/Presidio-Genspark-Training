using System;
using JobPortalAPI.Interfaces;
using BCrypt.Net;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using JobPortalAPI.Models.DTOs;
using JobPortalAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JobPortalAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<Guid, User> _userRepository;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            IRepository<Guid, User> userRepository,
            ILogger<AuthenticationService> logger,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<AuthResultDto> LoginAsync(UserAddRequestDto userDto)
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                if (users == null || !users.Any())
                {
                    _logger.LogWarning("Login failed - no users found in the system");
                    throw new Exception("Invalid username or password");
                }
                var user = users.FirstOrDefault(u => u.Email == userDto.Email && u.IsDeleted == false);
                if (user == null)
                {
                    _logger.LogWarning($"Login failed - user not found: {userDto.Email}");
                    throw new Exception("Invalid username or password");
                }
                Console.WriteLine($"User found: {user.Email}");

                if (string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(user.PasswordHash))
                {
                    _logger.LogWarning($"Login failed - missing password or hash for user: {userDto.Email}");
                    throw new Exception("Invalid username or password");
                }

                if (!BCrypt.Net.BCrypt.EnhancedVerify(userDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning($"Login failed - invalid password for user: {userDto.Email}");
                    throw new Exception("Invalid username or password");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Keys:JwtTokenKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: claims,
                    signingCredentials: creds
                );
                _logger.LogInformation("User {Username} logged in successfully", userDto.Email);
                var refreshToken = Guid.NewGuid().ToString();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userRepository.UpdateAsync(user);

                return new AuthResultDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    ExpiresAt = token.ValidTo,
                    Email = user.Email
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Username}", userDto.Email);
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuthResultDto> RefreshTokenAsync(TokenRefreshDto refreshDto)
        {
            try
            {
                if (refreshDto == null || string.IsNullOrEmpty(refreshDto.RefreshToken))
                {
                    _logger.LogWarning("Refresh token failed - missing refresh token or user ID");
                    throw new UnauthorizedAccessException("Invalid refresh token request");
                }

                var users = await _userRepository.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Id == refreshDto.UserId);

                if (user == null)
                {
                    _logger.LogWarning("Refresh token failed - user not found with ID: {UserId}", refreshDto.UserId);
                    throw new UnauthorizedAccessException("User not found");
                }

                if (user.RefreshToken != refreshDto.RefreshToken)
                {
                    _logger.LogWarning("Refresh token failed - token mismatch for user: {UserId}", user.Id);
                    _logger.LogInformation("Expected: {StoredToken}, Received: {ProvidedToken}", user.RefreshToken, refreshDto.RefreshToken);
                    throw new UnauthorizedAccessException("Refresh token mismatch");
                }

                if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    _logger.LogWarning("Refresh token failed - token expired for user: {UserId}", user.Id);
                    _logger.LogInformation("Token expired at: {ExpiryTime}, Current time: {Now}", user.RefreshTokenExpiryTime, DateTime.UtcNow);
                    throw new UnauthorizedAccessException("Refresh token expired");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Keys:JwtTokenKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: claims,
                    signingCredentials: creds
                );

                var newRefreshToken = Guid.NewGuid().ToString();
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userRepository.UpdateAsync(user);

                _logger.LogInformation("Refresh token successful for user: {UserId}", user.Id);

                return new AuthResultDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = newRefreshToken,
                    ExpiresAt = token.ValidTo,
                    Email = user.Email
                };
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during refresh token for user {UserId}", refreshDto?.UserId);
                throw new Exception("An error occurred during token refresh. Please try again later.");
            }
        }
        public async Task LogoutAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Logout failed - userId is null or empty");
                    throw new ArgumentException("User ID is required");
                }

                var users = await _userRepository.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Id.ToString() == userId);

                if (user == null)
                {
                    _logger.LogWarning("Logout failed - user not found with ID: {UserId}", userId);
                    throw new UnauthorizedAccessException("User not found");
                }
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userRepository.UpdateAsync(user);

                _logger.LogInformation("User {UserId} logged out successfully", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout for user {UserId}", userId);
                throw new Exception("An error occurred during logout. Please try again later.");
            }
        }


        public async Task<UserAddRequestDto?> GetUserByIdAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("GetUserById failed - userId is null or empty");
                    return null;
                }

                var users = await _userRepository.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Id.ToString() == userId);

                if (user == null)
                {
                    _logger.LogWarning("GetUserById failed - user not found with ID: {UserId}", userId);
                    return null;
                }
                return new UserAddRequestDto
                {
                    Email = user.Email,
                    Role = user.Role,
                    Password = user.PasswordHash
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID {UserId}", userId);
                throw new Exception("An error occurred while retrieving the user.");
            }
        }

    }
}
