using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.Models
{
    public class JobSeeker
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public Guid ResumeId { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public ICollection<ResumeDocument> ResumeDocuments { get; set; } = new List<ResumeDocument>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
