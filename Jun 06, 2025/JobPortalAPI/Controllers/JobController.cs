using JobPortalAPI.DTOs;
using JobPortalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models.DTOs;
using JobPortalAPI.Models;
using JobPortalAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly IJobServices _jobService;
        private readonly ILogger<JobController> _logger;
        private readonly IHubContext<JobHub> _hubContext;

        public JobController(IJobServices jobService, IHubContext<JobHub> hubContext, ILogger<JobController> logger)
        {
            _jobService = jobService;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Authorize(Roles = "JobSeeker, Admin, Recruiter")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all jobs");
            _logger.LogInformation("User {User} is fetching all jobs", User.Identity?.Name);
            var jobs = await _jobService.GetAllJobsAsync();
            if (jobs == null)
            {
                _logger.LogWarning("No jobs found");
                return NotFound("No jobs available");
            }
            _logger.LogInformation("Successfully fetched {Count} jobs");
            _logger.LogInformation("User {User} successfully fetched all jobs", User.Identity?.Name);
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Recruiter, JobSeeker")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching job with ID {JobId}", id);
            _logger.LogInformation("User {User} is fetching job with ID {JobId}", User.Identity?.Name, id);
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid job ID provided: {JobId}", id);
                return BadRequest("Invalid job ID");
            }
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            _logger.LogInformation("Successfully fetched job with ID {JobId}", id);
            _logger.LogInformation("User {User} successfully fetched job with ID {JobId}", User.Identity?.Name, id);
            return Ok(job);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> Create([FromBody] JobAddRequestDto dto)
        {
            _logger.LogInformation("Creating a new job");
            _logger.LogInformation("User {User} is creating a new job", User.Identity?.Name);
            var created = await _jobService.CreateJobAsync(dto);
            Console.WriteLine(created);
            if (created == null)
            {
                _logger.LogError("Failed to create job");
                return BadRequest("Job creation failed");
            }
            _logger.LogInformation("Successfully created job with ID {JobId}", User.Identity?.Name);
            _logger.LogInformation("User {User} successfully created a new job with ID {JobId}", User.Identity?.Name);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
            {
                Title = created.Title,
                CompanyName = created.CompanyName,
                Description = created.Description,
                Salary = created.Salary
            });
            return CreatedAtAction(nameof(GetById), new { id = Guid.NewGuid() }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> Update(Guid id, [FromBody] JobAddRequestDto dto)
        {
            _logger.LogInformation("Updating job with ID {JobId}", id);
            _logger.LogInformation("User {User} is updating job with ID {JobId}", User.Identity?.Name, id);
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid job ID provided for update: {JobId}", id);
                return BadRequest("Invalid job ID");
            }
            var updated = await _jobService.UpdateJobAsync(id, dto);
            if (updated == null) return NotFound();
            _logger.LogInformation("Successfully updated job with ID {JobId}", id);
            _logger.LogInformation("User {User} successfully updated job with ID {JobId}", User.Identity?.Name, id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting job with ID {JobId}", id);
            _logger.LogInformation("User {User} is deleting job with ID {JobId}", User.Identity?.Name, id);
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid job ID provided for deletion: {JobId}", id);
                return BadRequest("Invalid job ID");
            }
            var deleted = await _jobService.DeleteJobAsync(id);
            if (!deleted) return NotFound();
            _logger.LogInformation("Successfully deleted job with ID {JobId}", id);
            _logger.LogInformation("User {User} successfully deleted job with ID {JobId}", User.Identity?.Name, id);
            return NoContent();
        }
        [HttpPost("apply")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> ApplyJobs([FromBody] JobApplicationAddDto request)
        {
            _logger.LogInformation("User {User} is applying for job with ID {JobId}", User.Identity?.Name, request.JobId);

            if (request.JobId == Guid.Empty || request.JobSeekerId == Guid.Empty || request.ResumeDocumentId == Guid.Empty)
            {
                _logger.LogWarning("Invalid input provided. jobId: {JobId}, jobSeekerId: {JobSeekerId}, resumeId: {ResumeId}",
                    request.JobId, request.JobSeekerId, request.ResumeDocumentId);
                return BadRequest("All parameters (jobId, jobSeekerId, resumeId) are required.");
            }

            var result = await _jobService.ApplyForJobWithResumeIdAsync(request.JobId, request.JobSeekerId, request.ResumeDocumentId);

            if (result == "Application submitted successfully.")
            {
                _logger.LogInformation("User {User} successfully applied for job with ID {JobId}", User.Identity?.Name, request.JobId);
                return Ok(new { message = result });
            }
            else
            {
                _logger.LogWarning("Application failed: {Reason}", result);
                return BadRequest(new { error = result });
            }
        }
    }
}
