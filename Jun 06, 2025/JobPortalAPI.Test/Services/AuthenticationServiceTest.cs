using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using JobPortalAPI.Services;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using Microsoft.IdentityModel.Tokens;

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

            _configMock.Setup(x => x["Keys:JwtTokenKey"]).Returns("supersecretkey!123");

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
            var password = "password123";
            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Email = "test@example.com", PasswordHash = passwordHash, Role = "User" };

            _userRepoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<User> { user });

            var loginDto = new UserAddRequestDto { Email = user.Email, Password = password };

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedAccessException()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "user@example.com",
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("correct"),
                Role = "User"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            var dto = new UserAddRequestDto
            {
                Email = "user@example.com",
                Password = "wrong"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(dto));
        }

        [Fact]
        public async Task RefreshTokenAsync_ValidToken_ReturnsNewAuthResult()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "test@example.com",
                PasswordHash = "hashed",
                RefreshToken = "validtoken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                Role = "User"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            var refreshDto = new TokenRefreshDto { UserId = userId, RefreshToken = "validtoken" };

            var result = await _authService.RefreshTokenAsync(refreshDto);

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            Assert.False(string.IsNullOrEmpty(result.Token));
        }

        [Fact]
        public async Task RefreshTokenAsync_ExpiredToken_ThrowsUnauthorizedAccessException()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "expired@example.com",
                RefreshToken = "expiredtoken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-5),
                Role = "User"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            var dto = new TokenRefreshDto { UserId = userId, RefreshToken = "expiredtoken" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.RefreshTokenAsync(dto));
        }

        [Fact]
        public async Task LogoutAsync_ValidUser_RemovesToken()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "logout@example.com",
                RefreshToken = "token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1)
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            await _authService.LogoutAsync(userId.ToString());

            _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.RefreshToken == null)), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUserDto()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "user@example.com",
                Role = "User",
                PasswordHash = "hashed"
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { user });

            var result = await _authService.GetUserByIdAsync(userId.ToString());

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserNotFound_ReturnsNull()
        {
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

            var result = await _authService.GetUserByIdAsync(Guid.NewGuid().ToString());

            Assert.Null(result);
        }
    }
}
