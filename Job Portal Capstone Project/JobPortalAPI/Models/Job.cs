using System;
using System.Collections.Generic;

namespace JobPortalAPI.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CompanyName { get; set; }
        public decimal Salary { get; set; }
        public string Requirements { get; set; }

        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }

        public Guid RecruiterId { get; set; }
        public Recruiter? PostedBy { get; set; }

        public ICollection<JobEmploymentType> JobEmploymentTypes { get; set; } = new List<JobEmploymentType>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
