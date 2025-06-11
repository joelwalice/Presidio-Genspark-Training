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
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB per test
            .Options;
        return new JobContexts(options);
    }

    private Recruiter CreateRecruiter(Guid? id = null, Guid? companyId = null)
    {
        return new Recruiter
        {
            Id = id ?? Guid.NewGuid(),
            Name = "John Recruiter",
            Email = "john@example.com",
            PhoneNumber = "1234567890",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CompanyId = companyId ?? Guid.NewGuid()
        };
    }

    [Fact]
    public async Task AddAsync_ShouldAddRecruiter()
    {
        var context = GetInMemoryDbContext();
        var repo = new RecruiterRepository(context);

        var recruiter = CreateRecruiter();
        var result = await repo.AddAsync(recruiter);

        var dbRecruiter = await context.Recruiters.FindAsync(recruiter.Id);
        Assert.NotNull(dbRecruiter);
        Assert.Equal("John Recruiter", dbRecruiter.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecruiter()
    {
        var context = GetInMemoryDbContext();
        var recruiter = CreateRecruiter();
        context.Recruiters.Add(recruiter);
        await context.SaveChangesAsync();

        var repo = new RecruiterRepository(context);
        var result = await repo.GetByIdAsync(recruiter.Id);

        Assert.NotNull(result);
        Assert.Equal(recruiter.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRecruiters()
    {
        var context = GetInMemoryDbContext();
        context.Recruiters.Add(CreateRecruiter());
        context.Recruiters.Add(CreateRecruiter());
        await context.SaveChangesAsync();

        var repo = new RecruiterRepository(context);
        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyRecruiter()
    {
        var context = GetInMemoryDbContext();
        var recruiter = CreateRecruiter();
        context.Recruiters.Add(recruiter);
        await context.SaveChangesAsync();

        recruiter.Name = "Updated Name";
        var repo = new RecruiterRepository(context);
        var updated = await repo.UpdateAsync(recruiter);

        Assert.Equal("Updated Name", updated.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecruiter()
    {
        var context = GetInMemoryDbContext();
        var recruiter = CreateRecruiter();
        context.Recruiters.Add(recruiter);
        await context.SaveChangesAsync();

        var repo = new RecruiterRepository(context);
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
        var recruiter = CreateRecruiter(); // Not added

        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAsync(recruiter));
    }
}
