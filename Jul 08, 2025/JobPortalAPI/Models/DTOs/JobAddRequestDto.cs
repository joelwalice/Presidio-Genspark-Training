using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.DTOs
{
    public class JobAddRequestDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public decimal Salary { get; set; }

        public string Requirements { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public Guid RecruiterId { get; set; }
    }
}
