using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;

namespace BlobAPI.Services
{
    public class BlobService
    {
        private BlobContainerClient _containerClinet;
        private readonly IConfiguration _configuration;

        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        private async Task UpdateContainerClient()
        {
            var keyVaultUrl = _configuration["AzureBlob:KeyVaultUrl"];
            var secretName = _configuration["AzureBlob:SasSecretName"];

            if (string.IsNullOrWhiteSpace(keyVaultUrl))
                throw new Exception("Missing 'AzureBlob:KeyVaultUrl' in config.");

            if (string.IsNullOrWhiteSpace(secretName))
                throw new Exception("Missing 'AzureBlob:SasSecretName' in config.");

            Console.WriteLine($"🔐 Fetching secret '{secretName}' from KeyVault: {keyVaultUrl}");

            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

            KeyVaultSecret secret = await client.GetSecretAsync(secretName); // <-- 400 error if secretName is wrong
            var sasUrl = secret?.Value;

            if (string.IsNullOrWhiteSpace(sasUrl))
                throw new Exception("The secret value is empty or null.");

            if (!Uri.IsWellFormedUriString(sasUrl, UriKind.Absolute))
                throw new UriFormatException("Secret is not a valid URI: " + sasUrl);

            _containerClinet = new BlobContainerClient(new Uri(sasUrl));
        }


        public async Task UploadFile(Stream fileStream, string fileName)
        {
            await UpdateContainerClient();
            var blobClient = _containerClinet.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }

        public async Task<Stream> DownloadFile(string fileName)
        {
            await UpdateContainerClient();
            var blobClient = _containerClinet?.GetBlobClient(fileName);
            if (await blobClient.ExistsAsync())
            {
                var downloadInfor = await blobClient.DownloadStreamingAsync();
                return downloadInfor.Value.Content;
            }
            return null;
        }
    }
}
