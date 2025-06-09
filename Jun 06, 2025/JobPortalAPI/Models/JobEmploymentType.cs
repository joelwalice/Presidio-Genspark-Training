using System;

namespace JobPortalAPI.Models
{
    public class JobEmploymentType
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }
        public Job? Job { get; set; }

        public Guid EmploymentTypeId { get; set; }
        public EmploymentType? EmploymentType { get; set; }
    }
}
