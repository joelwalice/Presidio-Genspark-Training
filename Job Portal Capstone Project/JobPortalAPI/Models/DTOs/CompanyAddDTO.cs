using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.Models.DTOs
{
    public class CompanyAddDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public DateTime EstablishedDate { get; set; }
    }
}
