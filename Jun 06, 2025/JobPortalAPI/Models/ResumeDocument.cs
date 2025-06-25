using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.Models
{
    public class ResumeDocument
    {
        public Guid Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FileType { get; set; } = string.Empty;

        [Required]
        public byte[] Content { get; set; } = Array.Empty<byte>();

        public long FileSize { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid JobSeekerId { get; set; }

        public JobSeeker? JobSeeker { get; set; }
    }
}
