using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class JobApplicationController : ControllerBase
{
    private readonly IJobApplicationService _jobApplicationService;

    public JobApplicationController(IJobApplicationService jobApplicationService)
    {
        _jobApplicationService = jobApplicationService;
    }

    [HttpGet("applied/{jobSeekerId}")]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> GetAppliedJobs(Guid jobSeekerId)
    {
        var jobs = await _jobApplicationService.GetAppliedJobsByJobSeekerIdAsync(jobSeekerId);
        Console.WriteLine("Jobs fetched for Job Seeker ID: " + jobSeekerId);
        if (jobs == null || !jobs.Any())
        {
            return NotFound(new { Message = "No applied jobs found for this Job Seeker." });
        }
        Console.WriteLine(jobs);
        return Ok(jobs);
    }
    [HttpGet("job/{jobId}")]
    public async Task<IActionResult> GetApplicationsByJobId(Guid jobId)
    {
        var result = await _jobApplicationService.GetApplicationsByJobIdAsync(jobId);
        return Ok(result);
    }
    [HttpPut("{Id}/status")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> UpdateApplicationStatus(Guid Id, [FromBody] UpdateJobStatusDto dto)
    {
        try
        {
            var updatedApplication = await _jobApplicationService.UpdateApplicationStatusAsync(Id, dto);
            Console.WriteLine("-----> " + updatedApplication);
            
            return Ok(updatedApplication);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating application status: " + ex.Message);
            return BadRequest(new { Message = "Failed to update application status." });
        }
    }
}
