using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace JobPortalAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Guid, Company> _companyRepository;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(IRepository<Guid, Company> companyRepository, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }

        public async Task<CompanyDTO> GetCompanyByIdAsync(Guid id)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(id);
                if (company == null) return null;

                return new CompanyDTO
                {
                    Id = company.Id,
                    Name = company.Name,
                    Email = company.Email,
                    PhoneNumber = company.PhoneNumber,
                    Address = company.Address,
                    EstablishedDate = company.EstablishedDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get company by ID: {id}");
                return null;
            }
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync()
        {
            try
            {
                var companies = await _companyRepository.GetAllAsync();
                return companies.Select(company => new CompanyDTO
                {
                    Id = company.Id,
                    Name = company.Name,
                    Email = company.Email,
                    PhoneNumber = company.PhoneNumber,
                    Address = company.Address,
                    EstablishedDate = company.EstablishedDate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all companies.");
                return Enumerable.Empty<CompanyDTO>();
            }
        }

        public async Task<Company> AddCompanyAsync(CompanyAddDTO companyDto)
        {
            try
            {
                var company = new Company
                {
                    Id = Guid.NewGuid(),
                    Name = companyDto.Name,
                    Email = companyDto.Email,
                    PhoneNumber = companyDto.PhoneNumber,
                    Address = companyDto.Address,
                    EstablishedDate = companyDto.EstablishedDate,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                return await _companyRepository.AddAsync(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add new company.");
                return null;
            }
        }

        public async Task<Company> UpdateCompanyAsync(CompanyUpdateDTO companyDto)
        {
            try
            {
                var existing = await _companyRepository.GetByIdAsync(companyDto.Id);
                if (existing == null)
                {
                    _logger.LogWarning($"Company with ID {companyDto.Id} not found for update.");
                    return null;
                }

                existing.Name = companyDto.Name;
                existing.Email = companyDto.Email;
                existing.PhoneNumber = companyDto.PhoneNumber;
                existing.Address = companyDto.Address;
                existing.EstablishedDate = companyDto.EstablishedDate;
                existing.UpdatedAt = DateTime.UtcNow;

                return await _companyRepository.UpdateAsync(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update company with ID: {companyDto.Id}");
                return null;
            }
        }

        public async Task<bool> DeleteCompanyAsync(Guid id)
        {
            try
            {
                return await _companyRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete company with ID: {id}");
                return false;
            }
        }

        public async Task<IEnumerable<Recruiter>> GetRecruitersByCompanyIdAsync(Guid companyId)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(companyId);
                return company?.Recruiters ?? new List<Recruiter>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get recruiters for company ID: {companyId}");
                return Enumerable.Empty<Recruiter>();
            }
        }

        public async Task<IEnumerable<Job>> GetJobsByCompanyIdAsync(Guid companyId)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(companyId);
                return company?.Jobs ?? new List<Job>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get jobs for company ID: {companyId}");
                return Enumerable.Empty<Job>();
            }
        }
    }
}
