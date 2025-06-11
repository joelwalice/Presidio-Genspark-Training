using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;

namespace JobPortalAPI.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDTO> GetCompanyByIdAsync(Guid id);
        Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync();
        Task<Company> AddCompanyAsync(CompanyAddDTO companyDto);
        Task<Company> UpdateCompanyAsync(CompanyUpdateDTO companyDto);
        Task<bool> DeleteCompanyAsync(Guid id);
        Task<IEnumerable<Recruiter>> GetRecruitersByCompanyIdAsync(Guid companyId);
        Task<IEnumerable<Job>> GetJobsByCompanyIdAsync(Guid companyId);
    }
}
