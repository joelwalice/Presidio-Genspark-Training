using System.Threading.Tasks;
using DocumentSharing.Models;

namespace DocumentSharing.Interfaces
{
    public interface INotificationService
    {
        Task NotifyDocumentUploadAsync(Document document);
    }
}
