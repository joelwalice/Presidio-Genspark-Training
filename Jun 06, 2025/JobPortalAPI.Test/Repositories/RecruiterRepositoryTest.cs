using System;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Models;
using JobPortalAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class RecruiterRepositoryTests
{
    private JobContexts GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<JobContexts>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new JobContexts(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddRecruiter()
    {
        var context = GetInMemoryDbContext();
        var repo = new RecruiterRepository(context);

        var recruiter = new Recruiter
        {
            Id = Guid.NewGuid(),
            Name = "John Recruiter",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            CompanyId = Guid.NewGuid()
        };

        var result = await repo.AddAsync(recruiter);

        var dbRecruiter = await context.Recruiters.FindAsync(recruiter.Id);
        Assert.NotNull(dbRecruiter);
        Assert.Equal("John Recruiter", dbRecruiter.Name);
    }

    [Fact]
public async Task GetByIdAsync_ShouldReturnRecruiter()
{
    var context = GetInMemoryDbContext();
    var repo = new RecruiterRepository(context);

    var company = new Company
    {
        Id = Guid.NewGuid(),
        Name = "TestCorp",
        Email = "testcorp@example.com",
        Address = "India"
    };
    await context.Companies.AddAsync(company);
    await context.SaveChangesAsync();

    var recruiter = new Recruiter
    {
        Id = Guid.NewGuid(),
        Name = "Jane Recruiter",
        Email = "jane@example.com",
        PhoneNumber = "9876543210",
        CompanyId = company.Id
    };

    await repo.AddAsync(recruiter);

    var result = await repo.GetByIdAsync(recruiter.Id);

    Assert.NotNull(result);
    Assert.Equal(recruiter.Id, result.Id);
}


[Fact]
public async Task GetAllAsync_ShouldReturnAllRecruiters()
{
    var context = GetInMemoryDbContext();
    var repo = new RecruiterRepository(context);

    var company = new Company
    {
        Id = Guid.NewGuid(),
        Name = "TestCorp",
        Email = "info@testcorp.com",
        Address = "Chennai"
    };
    await context.Companies.AddAsync(company);
    await context.SaveChangesAsync();

    var recruiter1 = new Recruiter
    {
        Id = Guid.NewGuid(),
        Name = "Alice",
        Email = "alice@example.com",
        PhoneNumber = "1111111111",
        CompanyId = company.Id
    };

    var recruiter2 = new Recruiter
    {
        Id = Guid.NewGuid(),
        Name = "Bob",
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
    public async Task UpdateAsync_ShouldModifyRecruiter()
    {
        var context = GetInMemoryDbContext();
        var repo = new RecruiterRepository(context);

        var recruiter = new Recruiter
        {
            Id = Guid.NewGuid(),
            Name = "Temp Name",
            Email = "temp@example.com",
            PhoneNumber = "0000000000",
            CompanyId = Guid.NewGuid()
        };

        await repo.AddAsync(recruiter);

        recruiter.Name = "Updated Name";
        var updated = await repo.UpdateAsync(recruiter);

        Assert.Equal("Updated Name", updated.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecruiter()
    {
        var context = GetInMemoryDbContext();
        var repo = new RecruiterRepository(context);

        var recruiter = new Recruiter
        {
            Id = Guid.NewGuid(),
            Name = "To Delete",
            Email = "delete@example.com",
            PhoneNumber = "3333333333",
            CompanyId = Guid.NewGuid()
        };

        await repo.AddAsync(recruiter);

        var result = await repo.DeleteAsync(recruiter.Id);

        Assert.True(result);
        Assert.Null(await context.Recruiters.FindAsync(recruiter.Id));
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfNotFound()
    {
        var context = GetInMemoryDbContext();
        var repo = new RecruiterRepository(context);

        var result = await repo.DeleteAsync(Guid.NewGuid());
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenNotFound()
    {
        var context = GetInMemoryDbContext();
        var repo = new RecruiterRepository(context);

        var recruiter = new Recruiter
        {
            Id = Guid.NewGuid(),
            Name = "Ghost",
            Email = "ghost@example.com",
            PhoneNumber = "0000000000",
            CompanyId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAsync(recruiter));
    }
}
