using JobPortalAPI.Contexts;
using JobPortalAPI.DTOs;
using JobPortalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortalAPI.Services
{
    public class JobServices : IJobServices
    {
        private readonly JobContexts _context;

        public JobServices(JobContexts context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobGetRequestDto>> GetAllJobsAsync()
        {
            return await _context.Jobs
                .Select(j => new JobGetRequestDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    ExpiryDate = j.ExpiryDate,
                    CompanyName = j.CompanyName,
                    Salary = j.Salary,
                    Requirements = j.Requirements,
                    CompanyId = j.CompanyId,
                    RecruiterId = j.RecruiterId
                }).ToListAsync();
        }

        public async Task<JobGetRequestDto?> GetJobByIdAsync(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null) return null;

            return new JobGetRequestDto
            {
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                ExpiryDate = job.ExpiryDate,
                CompanyName = job.CompanyName,
                Salary = job.Salary,
                Requirements = job.Requirements,
                CompanyId = job.CompanyId,
                RecruiterId = job.RecruiterId
            };
        }

        public async Task<JobAddRequestDto> CreateJobAsync(JobAddRequestDto dto)
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                PostedDate = DateTime.UtcNow,
                ExpiryDate = dto.ExpiryDate?.ToUniversalTime(),
                CompanyName = dto.CompanyName,
                Salary = dto.Salary,
                Requirements = dto.Requirements,
                CompanyId = dto.CompanyId,
                RecruiterId = dto.RecruiterId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<JobAddRequestDto?> UpdateJobAsync(Guid id, JobAddRequestDto dto)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null) return null;

            job.Title = dto.Title;
            job.Description = dto.Description;
            job.Location = dto.Location;
            job.ExpiryDate = dto.ExpiryDate?.ToUniversalTime();
            job.CompanyName = dto.CompanyName;
            job.Salary = dto.Salary;
            job.Requirements = dto.Requirements;
            job.CompanyId = dto.CompanyId;
            job.RecruiterId = dto.RecruiterId;
            job.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<bool> DeleteJobAsync(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null) return false;

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> ApplyForJobWithResumeIdAsync(Guid jobId, Guid jobSeekerId, Guid resumeDocumentId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null) return "Job not found.";

            var resume = await _context.ResumeDocuments.FindAsync(resumeDocumentId);
            if (resume == null || resume.JobSeekerId != jobSeekerId)
                return "Resume not found or does not belong to this job seeker.";

            var alreadyApplied = await _context.JobApplications
                .AnyAsync(a => a.JobId == jobId && a.JobSeekerId == jobSeekerId);

            if (alreadyApplied)
                return "You have already applied for this job.";

            var application = new JobApplication
            {
                Id = Guid.NewGuid(),
                JobId = jobId,
                JobSeekerId = jobSeekerId,
                ResumeDocumentId = resumeDocumentId,
                AppliedAt = DateTime.UtcNow,
                JobStatus = JobStatus.Applied
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            return "Application submitted successfully.";
        }
        public async Task<IEnumerable<JobApplicationViewDto>> GetApplicationsForJobAsync(Guid jobId)
        {
            return await _context.JobApplications
                .Where(a => a.JobId == jobId)
                .Include(a => a.User)
                .Include(a => a.ResumeDocument)
                .Select(a => new JobApplicationViewDto
                {
                    JobSeekerId = a.User!.Id,
                    JobSeekerName = a.User!.Name,
                    ResumeDocumentId = a.ResumeDocumentId,
                    AppliedAt = a.AppliedAt
                }).ToListAsync();
        }
    }
}
