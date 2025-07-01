using Google.Apis.Auth;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.Extensions.Logging;

namespace FirstAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, User> _userRepository;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ITokenService tokenService,
                                    IEncryptionService encryptionService,
                                    IRepository<string, User> userRepository,
                                    ILogger<AuthenticationService> logger)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<UserLoginResponse> Login(UserLoginRequest user)
        {
            var dbUser = await _userRepository.Get(user.Username);
            if (dbUser == null)
            {
                _logger.LogCritical("User not found");
                throw new Exception("No such user");
            }
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = user.Password,
                HashKey = dbUser.HashKey
            });
            for (int i = 0; i < encryptedData.EncryptedData.Length; i++)
            {
                if (encryptedData.EncryptedData[i] != dbUser.Password[i])
                {
                    _logger.LogError("Invalid login attempt");
                    throw new Exception("Invalid password");
                }
            }
            var token = await _tokenService.GenerateToken(dbUser);
            return new UserLoginResponse
            {
                Username = user.Username,
                Token = token,
            };
        }
        
        public async Task<UserLoginResponse> GoogleLogin(string googleToken)
        {
            // Validate the Google token and extract user info
            var payload = await Google.Apis.Auth.GoogleJsonWebSignature.ValidateAsync(googleToken);
            if (payload == null || string.IsNullOrEmpty(payload.Email))
            {
                _logger.LogError("Invalid Google token");
                throw new Exception("Invalid Google token");
            }

            var dbUser = await _userRepository.Get(payload.Email);
            if (dbUser == null)
            {
                _logger.LogCritical("Google user not found");
                throw new Exception("No such user");
            }

            var token = await _tokenService.GenerateToken(dbUser);

            return new UserLoginResponse
            {
                Username = dbUser.Username,
                Token = token,
            };
        }
    }
}