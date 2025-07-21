using JobPortalAPI.Contexts;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

public class JobApplicationService : IJobApplicationService
{
    private readonly JobContexts _context;

    public JobApplicationService(JobContexts context)
    {
        _context = context;
    }

    public async Task<List<AppliedJobDto>> GetAppliedJobsByJobSeekerIdAsync(Guid jobSeekerId)
    {
        return await _context.JobApplications
            .Where(ja => ja.JobSeekerId == jobSeekerId)
            .Include(ja => ja.Job)
            .Include(ja => ja.ResumeDocument)
            .Select(ja => new AppliedJobDto
            {
                JobApplicationId = ja.Id,
                JobId = ja.Job.Id,
                Title = ja.Job.Title,
                Description = ja.Job.Description,
                Location = ja.Job.Location,
                CompanyName = ja.Job.CompanyName,
                Salary = ja.Job.Salary,
                Requirements = ja.Job.Requirements,
                AppliedAt = ja.AppliedAt,
                JobStatus = (int)ja.JobStatus
            })
            .ToListAsync();
    }
    public async Task<List<JobApplicationDto>> GetApplicationsByJobIdAsync(Guid jobId)
    {
        return await _context.JobApplications
            .Where(a => a.JobId == jobId)
            .Select(a => new JobApplicationDto
            {
                Id = a.Id,
                JobId = a.JobId,
                JobSeekerId = a.JobSeekerId,
                ResumeDocumentId = a.ResumeDocumentId,
                AppliedAt = a.AppliedAt,
                JobStatus = (int)a.JobStatus
            })
            .ToListAsync();
    }
    public async Task<JobApplicationDto> UpdateApplicationStatusAsync(Guid Id, UpdateJobStatusDto jobStatus)
    {
        Console.WriteLine("Updating application status for Job Seeker ID: " + Id + " with status: " + jobStatus.JobStatus);
        var application = await _context.JobApplications
            .FirstOrDefaultAsync(a => a.Id == Id && a.JobStatus != JobStatus.Hired);

        if (application == null)
        {
            throw new Exception("Application not found or already hired.");
        }
        Console.WriteLine("Updating application status for Job Seeker ID: " + Id + "Application : " + application);

        application.JobStatus = jobStatus.JobStatus;
        _context.JobApplications.Update(application);
        await _context.SaveChangesAsync();

        return new JobApplicationDto
        {
            Id = application.Id,
            JobId = application.JobId,
            JobSeekerId = application.JobSeekerId,
            ResumeDocumentId = application.ResumeDocumentId,
            AppliedAt = application.AppliedAt,
            JobStatus = (int)application.JobStatus
        };
    }
}