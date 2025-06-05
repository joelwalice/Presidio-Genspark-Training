using System;
using System.Threading.Tasks;
using DocumentSharing.Interfaces;
using DocumentSharing.Models;
using Microsoft.AspNetCore.SignalR;
using DocumentSharing.Hubs;

namespace DocumentSharing.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        
        public async Task NotifyDocumentUploadAsync(Document document)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveDocumentUpload", new
            {
                document.FileName,
                document.UploadedAt,
                Message = "A new document has been uploaded."
            });
        }
    }
}