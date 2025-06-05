using System;

namespace DocumentSharing.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public byte[]? FileData { get; set; } = null;
        public User? User { get; set; } = null;
    }
}