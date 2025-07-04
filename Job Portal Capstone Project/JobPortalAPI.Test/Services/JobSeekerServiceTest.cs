using Xunit;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using JobPortalAPI.Contexts;
using JobPortalAPI.Models;
using JobPortalAPI.Models.DTOs;
using JobPortalAPI.Services;
using JobPortalAPI.Repositories;

public class JobSeekerServiceTests
{
    private JobContexts GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<JobContexts>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new JobContexts(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task AddJobSeekerAsync_ShouldAddAndReturnJobSeeker()
    {
        var context = GetInMemoryDbContext();
        var service = new JobSeekerService(context);

        var dto = new JobSeekerAddRequestDto
        {
            Name = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            DateOfBirth = new DateTime(2000, 1, 1),
            Address = "123 Test Lane",
            Password = "password123"
        };

        var result = await service.AddJobSeekerAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);
    }

    [Fact]
    public async Task GetJobSeekerByIdAsync_ShouldReturnJobSeekerDto()
    {
        var context = GetInMemoryDbContext();
        var service = new JobSeekerService(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "john@example.com",
            Name = "John",
            Role = "JobSeeker",
            PasswordHash = "hashedPassword",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var jobSeeker = new JobSeeker
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "John Doe",
            Email = "john@example.com",
            PhoneNumber = "1111111111",
            Address = "123 Street",
            PasswordHash = user.PasswordHash,
            DateOfBirth = new DateTime(1995, 5, 20),
            User = user
        };

        context.Users.Add(user);
        context.JobSeekers.Add(jobSeeker);
        await context.SaveChangesAsync();

        var result = await service.GetJobSeekerByIdAsync(jobSeeker.Id);

        Assert.NotNull(result);
        Assert.Equal("John Doe", result.Name);
    }

    [Fact]
public async Task GetAllAsync_ShouldReturnAllRecruiters()
{
    var context = GetInMemoryDbContext();
    var repo = new RecruiterRepository(context);

    // Add a dummy company because recruiter needs a valid CompanyId
    var company = new Company
    {
        Id = Guid.NewGuid(),
        Name = "Test Company",
        Email = "company@example.com",
        Address = "Test City",
        PhoneNumber = "+91 977583902"
    };
    await context.Companies.AddAsync(company);
    await context.SaveChangesAsync();

    var recruiter1 = new Recruiter
    {
        Id = Guid.NewGuid(),
        Name = "Alice Recruiter",
        Email = "alice@example.com",
        PhoneNumber = "1111111111",
        CompanyId = company.Id
    };

    var recruiter2 = new Recruiter
    {
        Id = Guid.NewGuid(),
        Name = "Bob Recruiter",
        Email = "bob@example.com",
        PhoneNumber = "2222222222",
        CompanyId = company.Id
    };

    await repo.AddAsync(recruiter1);
    await repo.AddAsync(recruiter2);

    var result = await repo.GetAllAsync();

    Assert.Equal(2, result.Count());
}


    [Fact]
    public async Task UpdateJobSeekerAsync_ShouldUpdateFields()
    {
        var context = GetInMemoryDbContext();
        var service = new JobSeekerService(context);

        var jobSeeker = new JobSeeker
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            PhoneNumber = "0000",
            Address = "Old Address"
        };
        context.JobSeekers.Add(jobSeeker);
        await context.SaveChangesAsync();

        var updateDto = new JobSeekerUpdateRequestDto
        {
            Id = jobSeeker.Id,
            Name = "New Name",
            PhoneNumber = "9999",
            Address = "New Address"
        };

        // Act
        var updated = await service.UpdateJobSeekerAsync(updateDto);

        // Assert
        Assert.Equal("New Name", updated.Name);
        Assert.Equal("9999", updated.PhoneNumber);
        Assert.Equal("New Address", updated.Address);
    }

    [Fact]
    public async Task DeleteJobSeekerAsync_ShouldRemoveJobSeeker()
    {
        var context = GetInMemoryDbContext();
        var service = new JobSeekerService(context);

        var jobSeeker = new JobSeeker { Id = Guid.NewGuid(), Name = "ToDelete" };
        context.JobSeekers.Add(jobSeeker);
        await context.SaveChangesAsync();

        // Act
        var result = await service.DeleteJobSeekerAsync(jobSeeker.Id);

        // Assert
        Assert.True(result);
        Assert.Null(await context.JobSeekers.FindAsync(jobSeeker.Id));
    }

    [Fact]
    public async Task GetResumeDocumentsByJobSeekerIdAsync_ShouldReturnResumes()
    {
        var context = GetInMemoryDbContext();
        var service = new JobSeekerService(context);

        var jobSeekerId = Guid.NewGuid();
        context.ResumeDocuments.Add(new ResumeDocument
        {
            Id = Guid.NewGuid(),
            JobSeekerId = jobSeekerId,
            FileName = "resume.pdf",
            FileType = "application/pdf",
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        // Act
        var resumes = await service.GetResumeDocumentsByJobSeekerIdAsync(jobSeekerId);

        // Assert
        Assert.NotNull(resumes);
        Assert.Single(resumes);
        Assert.Equal("resume.pdf", resumes.First().FileName);
    }
}
