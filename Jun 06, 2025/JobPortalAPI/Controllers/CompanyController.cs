using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JobPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Authorize(Roles="Admin, Recruiter")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        [Authorize(Roles="Admin, Recruiter")]
        public async Task<ActionResult<CompanyDTO>> GetCompanyById(Guid id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound($"Company with ID {id} not found.");

            return Ok(company);
        }

        [HttpPost]
        [Authorize(Roles="Admin, Recruiter")]
        public async Task<ActionResult<Company>> AddCompany([FromBody] CompanyAddDTO companyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.AddCompanyAsync(companyDto);
            if (result == null)
                return StatusCode(500, "Error while creating company.");

            return CreatedAtAction(nameof(GetCompanyById), new { id = result.Id }, result);
        }

        [HttpPut]
        [Authorize(Roles="Admin, Recruiter")]
        public async Task<ActionResult<Company>> UpdateCompany([FromBody] CompanyUpdateDTO companyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _companyService.UpdateCompanyAsync(companyDto);
            if (updated == null)
                return NotFound($"Company with ID {companyDto.Id} not found.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles="Admin, Recruiter")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var deleted = await _companyService.DeleteCompanyAsync(id);
            if (!deleted)
                return NotFound($"Company with ID {id} not found.");

            return NoContent();
        }

        [HttpGet("{id}/recruiters")]
        public async Task<ActionResult<IEnumerable<Recruiter>>> GetRecruitersByCompanyId(Guid id)
        {
            var recruiters = await _companyService.GetRecruitersByCompanyIdAsync(id);
            return Ok(recruiters);
        }

        [HttpGet("{id}/jobs")]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobsByCompanyId(Guid id)
        {
            var jobs = await _companyService.GetJobsByCompanyIdAsync(id);
            return Ok(jobs);
        }
    }
}
