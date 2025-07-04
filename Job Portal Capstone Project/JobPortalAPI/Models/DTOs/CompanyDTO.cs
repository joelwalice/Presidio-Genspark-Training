using System;

namespace JobPortalAPI.Models.DTOs
{
    public class CompanyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime EstablishedDate { get; set; }
    }
}
