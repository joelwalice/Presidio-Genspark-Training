using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobPortalAPI.Services;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;

namespace JobPortalAPI.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IRepository<Guid, User>> _userRepoMock;
        private readonly Mock<ILogger<AuthenticationService>> _loggerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly AuthenticationService _authService;

        public AuthenticationServiceTests()
        {
            _userRepoMock = new Mock<IRepository<Guid, User>>();
            _loggerMock = new Mock<ILogger<AuthenticationService>>();
            _configMock = new Mock<IConfiguration>();

            // Use valid 256-bit key for HMAC-SHA256
            _configMock.Setup(c => c["Keys:JwtTokenKey"])
                .Returns("32_character_supersecret_key_1234567890!");

            _authService = new AuthenticationService(
                _userRepoMock.Object,
                _loggerMock.Object,
                _configMock.Object
            );
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsAuthResult()
        {
            // Arrange
            var password = "securepassword";
            var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13); // Standard hash
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "user@example.com",
                PasswordHash = hashedPassword,
                Role = "User"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            var loginDto = new UserAddRequestDto
            {
                Email = user.Email,
                Password = password
            };

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
            Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "user@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpass"),
                Role = "User"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            var dto = new UserAddRequestDto
            {
                Email = user.Email,
                Password = "wrongpass"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(dto));
        }

        [Fact]
        public async Task RefreshTokenAsync_ValidToken_ReturnsNewAuthResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "refresh@example.com",
                RefreshToken = "validtoken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1),
                Role = "User"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            var dto = new TokenRefreshDto
            {
                UserId = userId,
                RefreshToken = "validtoken"
            };

            // Act
            var result = await _authService.RefreshTokenAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            Assert.False(string.IsNullOrEmpty(result.Token));
        }

        [Fact]
        public async Task RefreshTokenAsync_Expired_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "expired@example.com",
                RefreshToken = "expired",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-5),
                Role = "User"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            var dto = new TokenRefreshDto
            {
                UserId = userId,
                RefreshToken = "expired"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.RefreshTokenAsync(dto));
        }

        [Fact]
        public async Task LogoutAsync_ValidUser_ClearsRefreshToken()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "logout@example.com",
                RefreshToken = "some-token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                Role = "User"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            // Act
            await _authService.LogoutAsync(userId.ToString());

            // Assert
            _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
                u.Id == userId && u.RefreshToken == null
            )), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "check@example.com",
                Role = "User"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            // Act
            var result = await _authService.GetUserByIdAsync(userId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserNotFound_ReturnsNull()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

            // Act
            var result = await _authService.GetUserByIdAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.Null(result);
        }
    }
}
