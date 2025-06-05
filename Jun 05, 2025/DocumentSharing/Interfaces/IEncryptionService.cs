using System.Threading.Tasks;

namespace DocumentSharing.Interfaces
{
    public class EncryptModel
    {
        public string Data { get; set; }
        public byte[] HashKey { get; set; }
        public byte[] EncryptedData { get; set; }
    }

    public interface IEncryptionService
    {
        Task<EncryptModel> EncryptData(EncryptModel model);
        Task<EncryptModel> DecryptData(EncryptModel model);
    }
}
