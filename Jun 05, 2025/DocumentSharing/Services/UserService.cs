using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentSharing.Interfaces;
using DocumentSharing.Models;
using DocumentSharing.Contexts;
using DocumentSharing.Repositories;
using Microsoft.Extensions.Logging;

namespace DocumentSharing.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IRepository<int, User> userRepository,
            IEncryptionService encryptionService,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _logger = logger;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {Id} not found", id);
                    throw new KeyNotFoundException($"User with ID {id} not found");
                }
                return user;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error getting user with ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                ValidateUser(user);

                // Encrypt password
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = user.Password
                });
                user.Password = encryptedData.EncryptedData;
                user.HashKey = encryptedData.HashKey;

                // Set default values
                user.CreatedAt = DateTime.UtcNow;
                if (string.IsNullOrEmpty(user.Role))
                {
                    user.Role = "user";
                }

                return await _userRepository.Add(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user: {UserEmail}", user?.Email);
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                var existingUser = await GetUserByIdAsync(id);

                // Only update password if it's provided
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                    {
                        Data = user.Password
                    });
                    existingUser.Password = encryptedData.EncryptedData;
                    existingUser.HashKey = encryptedData.HashKey;
                }

                // Update other fields
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;

                return await _userRepository.Update(id, existingUser);
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error updating user with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await GetUserByIdAsync(id); // This will throw if user doesn't exist
                return await _userRepository.Delete(id);
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error deleting user with ID {Id}", id);
                throw;
            }
        }

        private void ValidateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("Email is required");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Password is required");
            }

            if (string.IsNullOrEmpty(user.Name))
            {
                throw new ArgumentException("Name is required");
            }
        }
    }
}