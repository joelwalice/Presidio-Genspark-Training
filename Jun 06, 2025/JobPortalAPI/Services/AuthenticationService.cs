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
                    throw new UnauthorizedAccessException("Invalid username or password");
                }
                var user = users.FirstOrDefault(u => u.Email == userDto.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Login failed - user not found: {userDto.Email}");
                    throw new UnauthorizedAccessException("Invalid username or password");
                }
                Console.WriteLine($"User found: {user.Email}");

                if (string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(user.PasswordHash))
                {
                    _logger.LogWarning($"Login failed - missing password or hash for user: {userDto.Email}");
                    throw new UnauthorizedAccessException("Invalid username or password");
                }

                if (!BCrypt.Net.BCrypt.EnhancedVerify(userDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning($"Login failed - invalid password for user: {userDto.Email}");
                    throw new UnauthorizedAccessException("Invalid username or password");
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
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Username}", userDto.Email);
                throw new Exception("An error occurred during login. Please try again later.");
            }
        }

        public async Task<AuthResultDto> RefreshTokenAsync(TokenRefreshDto refreshDto)
        {
            try
            {
                if (refreshDto == null || string.IsNullOrEmpty(refreshDto.RefreshToken))
                {
                    _logger.LogWarning("Refresh token failed - missing refresh token or email");
                    throw new UnauthorizedAccessException("Invalid refresh token request");
                }

                var users = await _userRepository.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Id == refreshDto.UserId);

                if (user == null || user.RefreshToken != refreshDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    _logger.LogWarning("Refresh token failed - invalid or expired refresh token for user: {Email}", refreshDto.UserId);
                    throw new UnauthorizedAccessException("Invalid or expired refresh token");
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

                // Optionally, generate a new refresh token
                var newRefreshToken = Guid.NewGuid().ToString();
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userRepository.UpdateAsync(user);

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
                _logger.LogError(ex, "Error during refresh token for user {Email}", refreshDto?.UserId);
                throw new Exception("An error occurred during token refresh. Please try again later.");
            }
        }

        public async Task LogoutAsync(string userId)
        {
            // Implementation for logging out user
            throw new NotImplementedException();
        }

        public async Task<UserAddRequestDto?> GetUserByIdAsync(string userId)
        {
            // Implementation for retrieving user by ID
            throw new NotImplementedException();
        }
    }
}
