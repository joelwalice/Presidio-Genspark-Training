using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Interfaces;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using JobPortalAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class CompanyServiceTests
{
    private readonly Mock<IRepository<Guid, Company>> _mockRepo;
    private readonly Mock<ILogger<CompanyService>> _mockLogger;
    private readonly CompanyService _service;

    public CompanyServiceTests()
    {
        _mockRepo = new Mock<IRepository<Guid, Company>>();
        _mockLogger = new Mock<ILogger<CompanyService>>();
        _service = new CompanyService(_mockRepo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCompanyByIdAsync_ShouldReturnCompanyDTO_IfExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var company = new Company
        {
            Id = id,
            Name = "ABC Corp",
            Email = "abc@corp.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St",
            EstablishedDate = new DateTime(2000, 1, 1)
        };
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(company);

        // Act
        var result = await _service.GetCompanyByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(company.Id, result.Id);
        Assert.Equal(company.Name, result.Name);
    }

    [Fact]
    public async Task GetCompanyByIdAsync_ShouldReturnNull_IfNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Company)null);

        // Act
        var result = await _service.GetCompanyByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllCompaniesAsync_ShouldReturnAllCompanies()
    {
        // Arrange
        var companies = new List<Company>
        {
            new Company { Id = Guid.NewGuid(), Name = "A", Email = "a@example.com" },
            new Company { Id = Guid.NewGuid(), Name = "B", Email = "b@example.com" }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(companies);

        // Act
        var result = await _service.GetAllCompaniesAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AddCompanyAsync_ShouldReturnCreatedCompany()
    {
        // Arrange
        var dto = new CompanyAddDTO
        {
            Name = "New Co",
            Email = "new@co.com",
            PhoneNumber = "9999999999",
            Address = "New St",
            EstablishedDate = DateTime.Today
        };

        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Company>()))
                 .ReturnsAsync((Company c) => c);

        // Act
        var result = await _service.AddCompanyAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Email, result.Email);
    }

    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnUpdatedCompany()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingCompany = new Company
        {
            Id = id,
            Name = "Old Co",
            Email = "old@co.com",
            PhoneNumber = "123",
            Address = "Old St",
            EstablishedDate = new DateTime(2000, 1, 1)
        };

        var updateDto = new CompanyUpdateDTO
        {
            Id = id,
            Name = "Updated Co",
            Email = "updated@co.com",
            PhoneNumber = "9999",
            Address = "Updated St",
            EstablishedDate = DateTime.Today
        };

        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingCompany);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Company>())).ReturnsAsync((Company c) => c);

        // Act
        var result = await _service.UpdateCompanyAsync(updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDto.Name, result.Name);
        Assert.Equal(updateDto.Email, result.Email);
    }

    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnNull_IfCompanyNotFound()
    {
        // Arrange
        var updateDto = new CompanyUpdateDTO { Id = Guid.NewGuid() };
        _mockRepo.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync((Company)null);

        // Act
        var result = await _service.UpdateCompanyAsync(updateDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteCompanyAsync_ShouldReturnTrue_OnSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteCompanyAsync(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCompanyAsync_ShouldReturnFalse_OnFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteCompanyAsync(id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetRecruitersByCompanyIdAsync_ShouldReturnRecruiters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var recruiters = new List<Recruiter>
        {
            new Recruiter { Id = Guid.NewGuid(), Name = "Recruiter1" }
        };

        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Company { Id = id, Recruiters = recruiters });

        // Act
        var result = await _service.GetRecruitersByCompanyIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetJobsByCompanyIdAsync_ShouldReturnJobs()
    {
        // Arrange
        var id = Guid.NewGuid();
        var jobs = new List<Job>
        {
            new Job { Id = Guid.NewGuid(), Title = "Software Engineer" }
        };

        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Company { Id = id, Jobs = jobs });

        // Act
        var result = await _service.GetJobsByCompanyIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
