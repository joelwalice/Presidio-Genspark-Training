using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerController : ControllerBase
    {
        private readonly IJobSeekerService _jobSeekerService;
        private readonly ILogger<JobSeekerController> _logger;

        public JobSeekerController(IJobSeekerService jobSeekerService, ILogger<JobSeekerController> logger)
        {
            _logger = logger;
            _jobSeekerService = jobSeekerService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,JobSeeker")]
        public async Task<IActionResult> GetJobSeekerById(Guid id)
        {
            _logger.LogInformation("Fetching job seeker with ID {JobSeekerId}", id);
            _logger.LogInformation("User {User} is fetching job seeker with ID {JobSeekerId}", User.Identity?.Name, id);
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid job seeker ID provided: {JobSeekerId}", id);
                return BadRequest("Invalid job seeker ID.");
            }
            var jobSeeker = await _jobSeekerService.GetJobSeekerByIdAsync(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Successfully fetched job seeker with ID {JobSeekerId}", id);
            _logger.LogInformation("User {User} successfully fetched job seeker with ID {JobSeekerId}", User.Identity?.Name, id);
            return Ok(jobSeeker);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,JobSeeker")]
        public async Task<IActionResult> GetAllJobSeekers()
        {
            _logger.LogInformation("Fetching all job seekers");
            _logger.LogInformation("User {User} is fetching all job seekers", User.Identity?.Name);
            var jobSeekers = await _jobSeekerService.GetAllJobSeekersAsync();
            if (jobSeekers == null || !jobSeekers.Any())
            {
                _logger.LogWarning("No job seekers found");
                return NotFound("No job seekers available.");
            }
            _logger.LogInformation("Successfully fetched {Count} job seekers");
            _logger.LogInformation("User {User} successfully fetched all job seekers", User.Identity?.Name);
            return Ok(jobSeekers);
        }

        [HttpPost]
        
        public async Task<IActionResult> AddJobSeeker([FromBody] JobSeekerAddRequestDto jobSeeker)
        {
            _logger.LogInformation("Adding a new job seeker");
            _logger.LogInformation("User {User} is adding a new job seeker", User.Identity?.Name);
            if (jobSeeker == null || string.IsNullOrWhiteSpace(jobSeeker.Name) || string.IsNullOrWhiteSpace(jobSeeker.Email))
            {
                _logger.LogWarning("Invalid job seeker data provided");
                return BadRequest("Invalid job seeker data.");
            }
            if (jobSeeker == null)
            {
                return BadRequest("Invalid job seeker data.");
            }
            var createdJobSeeker = await _jobSeekerService.AddJobSeekerAsync(jobSeeker);
            if (createdJobSeeker == null)
            {
                _logger.LogError("Failed to create job seeker");
                return BadRequest("Job seeker creation failed.");
            }
            _logger.LogInformation("Successfully added job seeker with ID {JobSeekerId}", createdJobSeeker.Id);
            _logger.LogInformation("User {User} successfully added a new job seeker with ID {JobSeekerId}", User.Identity?.Name, createdJobSeeker.Id);
            return CreatedAtAction(nameof(GetJobSeekerById), new { id = createdJobSeeker.Id }, createdJobSeeker);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,JobSeeker")]
        public async Task<IActionResult> UpdateJobSeeker([FromBody] JobSeekerUpdateRequestDto jobSeeker)
        {
            _logger.LogInformation("Updating job seeker with ID {JobSeekerId}", jobSeeker.Id);
            _logger.LogInformation("User {User} is updating job seeker with ID {JobSeekerId}", User.Identity?.Name, jobSeeker.Id);
            if (jobSeeker == null || string.IsNullOrWhiteSpace(jobSeeker.Name) || string.IsNullOrWhiteSpace(jobSeeker.Email))
            {
                _logger.LogWarning("Invalid job seeker data provided for update");
                return BadRequest("Invalid job seeker data.");
            }
            if (jobSeeker == null || jobSeeker.Id == Guid.Empty)
            {
                return BadRequest("Invalid job seeker data.");
            }
            var updatedJobSeeker = await _jobSeekerService.UpdateJobSeekerAsync(jobSeeker);
            if (updatedJobSeeker == null)
            {
                _logger.LogError("Failed to update job seeker with ID {JobSeekerId}", jobSeeker.Id);
                return NotFound("Job seeker not found.");
            }   
            _logger.LogInformation("Successfully updated job seeker with ID {JobSeekerId}", updatedJobSeeker.Id);
            _logger.LogInformation("User {User} successfully updated job seeker with ID {JobSeekerId}", User.Identity?.Name, updatedJobSeeker.Id);
            return Ok(updatedJobSeeker);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJobSeeker(Guid id)
        {
            _logger.LogInformation("Deleting job seeker with ID {JobSeekerId}", id);
            _logger.LogInformation("User {User} is deleting job seeker with ID {JobSeekerId}", User.Identity?.Name, id);
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid job seeker ID provided for deletion: {JobSeekerId}", id);
                return BadRequest("Invalid job seeker ID.");
            }
            await _jobSeekerService.DeleteJobSeekerAsync(id);
            _logger.LogInformation("Successfully deleted job seeker with ID {JobSeekerId}", id);
            _logger.LogInformation("User {User} successfully deleted job seeker with ID {JobSeekerId}", User.Identity?.Name, id);
            return NoContent();
        }

        [HttpGet("resumes/{jobSeekerId}")]
        [Authorize(Roles = "Admin,JobSeeker")]
        public async Task<IActionResult> GetResumeDocumentsByJobSeekerId(Guid jobSeekerId)
        {
            _logger.LogInformation("Fetching resume documents for job seeker with ID {JobSeekerId}", jobSeekerId);
            _logger.LogInformation("User {User} is fetching resume documents for job seeker with ID {JobSeekerId}", User.Identity?.Name, jobSeekerId);
            if (jobSeekerId == Guid.Empty)
            {
                _logger.LogWarning("Invalid job seeker ID provided for resume documents: {JobSeekerId}", jobSeekerId);
                return BadRequest("Invalid job seeker ID.");
            }
            var resumes = await _jobSeekerService.GetResumeDocumentsByJobSeekerIdAsync(jobSeekerId);
            if (resumes == null || !resumes.Any())
            {
                return NotFound();
            }
            _logger.LogInformation("Successfully fetched resume documents for job seeker with ID {JobSeekerId}", jobSeekerId);
            _logger.LogInformation("User {User} successfully fetched resume documents for job seeker with ID {JobSeekerId}", User.Identity?.Name, jobSeekerId);
            return Ok(resumes);
        }
    }
}