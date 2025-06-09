using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.DTOs
{
    public class ResumeUploadDto
    {
        [Required]
        public Guid JobSeekerId { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
