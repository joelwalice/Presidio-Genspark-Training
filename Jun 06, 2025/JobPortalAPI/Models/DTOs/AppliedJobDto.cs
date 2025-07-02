using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.Models.DTOs
{
    public class AppliedJobDto
    {
        public Guid JobApplicationId { get; set; }
        public Guid JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string CompanyName { get; set; }
        public decimal Salary { get; set; }
        public string Requirements { get; set; }
        public DateTime AppliedAt { get; set; }
        public int JobStatus { get; set; }
        public string ResumeUsed { get; set; }
    }
}