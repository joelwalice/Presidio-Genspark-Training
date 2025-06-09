using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;

namespace JobPortalAPI.Interfaces
{
    public interface IJobSeekerService
    {
        Task<JobSeekerAddRequestDto> GetJobSeekerByIdAsync(Guid id);
        Task<IEnumerable<JobSeekerAddRequestDto>> GetAllJobSeekersAsync();
        Task<JobSeeker> AddJobSeekerAsync(JobSeekerAddRequestDto jobSeeker);
        Task<JobSeeker> UpdateJobSeekerAsync(JobSeekerUpdateRequestDto jobSeeker);
        Task<bool> DeleteJobSeekerAsync(Guid id);
        Task<IEnumerable<ResumeDocument>> GetResumeDocumentsByJobSeekerIdAsync(Guid jobSeekerId);
    }
}