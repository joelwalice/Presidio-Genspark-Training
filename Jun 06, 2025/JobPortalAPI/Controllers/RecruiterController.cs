using System;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruiterController : ControllerBase
    {
        private readonly IRecruiterService _recruiterService;
        private readonly ILogger<RecruiterController> _logger;

        public RecruiterController(IRecruiterService recruiterService, ILogger<RecruiterController> logger)
        {
            _logger = logger;
            _recruiterService = recruiterService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<IActionResult> GetRecruiterById(Guid id)
        {
            _logger.LogInformation("Fetching recruiter with ID {RecruiterId}", id);
            _logger.LogInformation("User {User} is fetching recruiter with ID {RecruiterId}", User.Identity?.Name, id);
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid recruiter ID provided: {RecruiterId}", id);
                return BadRequest("Invalid recruiter ID.");
            }
            var recruiter = await _recruiterService.GetRecruiterByIdAsync(id);
            if (recruiter == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Successfully fetched recruiter with ID {RecruiterId}", id);
            _logger.LogInformation("User {User} successfully fetched recruiter with ID {RecruiterId}", User.Identity?.Name, id);
            return Ok(recruiter);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<IActionResult> GetAllRecruiters()
        {
            _logger.LogInformation("Fetching all recruiters");
            _logger.LogInformation("User {User} is fetching all recruiters", User.Identity?.Name);
            var recruiters = await _recruiterService.GetAllRecruitersAsync();
            if (recruiters == null || !recruiters.Any())
            {
                _logger.LogWarning("No recruiters found");
                return NotFound("No recruiters available.");
            }
            _logger.LogInformation("Successfully fetched {Count} recruiters");
            _logger.LogInformation("User {User} successfully fetched all recruiters", User.Identity?.Name);
            return Ok(recruiters);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecruiter([FromBody] RecruiterAddRequestDto recruiter)
        {
            _logger.LogInformation("Adding new recruiter");
            _logger.LogInformation("User {User} is adding a new recruiter", User.Identity?.Name);
            if (recruiter == null || string.IsNullOrWhiteSpace(recruiter.Name) || string.IsNullOrWhiteSpace(recruiter.Email))
            {
                _logger.LogWarning("Invalid recruiter data provided");
                return BadRequest("Invalid recruiter data.");
            }
            if (recruiter == null)
            {
                return BadRequest("Invalid recruiter data.");
            }
            var createdRecruiter = await _recruiterService.AddRecruiterAsync(recruiter);
            if (createdRecruiter == null)
            {
                _logger.LogError("Failed to create recruiter");
                return BadRequest("Failed to create recruiter.");
            }
            _logger.LogInformation("Successfully created recruiter with ID {RecruiterId}", createdRecruiter.Id);
            _logger.LogInformation("User {User} successfully added a new recruiter with ID {RecruiterId}", User.Identity?.Name, createdRecruiter.Id);
            return CreatedAtAction(nameof(GetRecruiterById), new { id = createdRecruiter.Id }, createdRecruiter);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<IActionResult> UpdateRecruiter([FromBody] RecruiterUpdateRequestDto recruiter)
        {
            _logger.LogInformation("Updating recruiter");
            _logger.LogInformation("User {User} is updating recruiter with ID {RecruiterId}", User.Identity?.Name, recruiter.Id);
            if (recruiter == null || string.IsNullOrWhiteSpace(recruiter.Name) || string.IsNullOrWhiteSpace(recruiter.Email))
            {
                _logger.LogWarning("Invalid recruiter data provided");
                return BadRequest("Invalid recruiter data.");
            }
            if (recruiter == null || recruiter.Id == Guid.Empty)
            {
                return BadRequest("Invalid recruiter data.");
            }
            var updatedRecruiter = await _recruiterService.UpdateRecruiterAsync(recruiter);
            if (updatedRecruiter == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Successfully updated recruiter with ID {RecruiterId}", recruiter.Id);
            _logger.LogInformation("User {User} successfully updated recruiter with ID {RecruiterId}", User.Identity?.Name, recruiter.Id);
            return Ok(updatedRecruiter);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRecruiter(Guid id)
        {
            _logger.LogInformation("Deleting recruiter with ID {RecruiterId}", id);
            _logger.LogInformation("User {User} is deleting recruiter with ID {RecruiterId}", User.Identity?.Name, id);
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid recruiter ID provided for deletion: {RecruiterId}", id);
                return BadRequest("Invalid recruiter ID.");
            }
            var result = await _recruiterService.DeleteRecruiterAsync(id);
            if (!result)
            {
                return NotFound();
            }
        _logger.LogInformation("Successfully deleted recruiter with ID {RecruiterId}", id);
            _logger.LogInformation("User {User} is deleted successfully!");
            return NoContent();
        }
    }
}