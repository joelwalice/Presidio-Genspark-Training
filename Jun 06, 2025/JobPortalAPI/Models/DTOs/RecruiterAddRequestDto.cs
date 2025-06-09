using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.Models.DTOs
{
    public class RecruiterAddRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}