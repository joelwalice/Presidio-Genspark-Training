using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DocumentSharing.Interfaces;
using Microsoft.Extensions.Logging;

namespace DocumentSharing.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly ILogger<EncryptionService> _logger;

        public EncryptionService(ILogger<EncryptionService> logger)
        {
            _logger = logger;
        }

        public async Task<EncryptModel> EncryptData(EncryptModel model)
        {
            try
            {
                using var aes = Aes.Create();
                aes.GenerateKey();
                model.HashKey = aes.Key;

                using var encryptor = aes.CreateEncryptor();
                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    await swEncrypt.WriteAsync(model.Data);
                }

                model.EncryptedData = msEncrypt.ToArray();
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error encrypting data");
                throw;
            }
        }

        public async Task<EncryptModel> DecryptData(EncryptModel model)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = model.HashKey;

                using var decryptor = aes.CreateDecryptor();
                using var msDecrypt = new MemoryStream(model.EncryptedData);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                
                model.Data = await srDecrypt.ReadToEndAsync();
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decrypting data");
                throw;
            }
        }
    }
}
