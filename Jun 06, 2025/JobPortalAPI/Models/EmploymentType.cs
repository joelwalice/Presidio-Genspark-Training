using System;

namespace JobPortalAPI.Models
{
    public class EmploymentType
    {
        public Guid JobId { get; set; }
        public Job? Job { get; set; }

        public Guid EmploymentTypeId { get; set; }
        public ICollection<JobEmploymentType> JobEmploymentTypes { get; set; } = new List<JobEmploymentType>();
    }

}