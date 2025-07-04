using System;
using System.Collections.Generic;

namespace JobPortalAPI.Models.DTOs
{
    public class JobSeekerUpdateRequestDto : JobSeekerAddRequestDto
    {
        public Guid Id { get; set; }
    }
}
