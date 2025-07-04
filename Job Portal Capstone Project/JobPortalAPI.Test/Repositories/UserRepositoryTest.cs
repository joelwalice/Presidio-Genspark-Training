using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Models;
using JobPortalAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserRepositoryTests
{
    private JobContexts GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<JobContexts>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new JobContexts(options);
    }

    private User CreateUser(Guid? id = null)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hashed_pw",
            Role = "JobSeeker",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        var context = GetInMemoryDbContext();
        var repo = new UserRepository(context);
        var user = CreateUser();

        var result = await repo.AddAsync(user);
        var dbUser = await context.Users.FindAsync(user.Id);

        Assert.NotNull(dbUser);
        Assert.Equal("Test User", dbUser.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        var context = GetInMemoryDbContext();
        var user = CreateUser();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(context);
        var result = await repo.GetByIdAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        var context = GetInMemoryDbContext();
        context.Users.Add(CreateUser());
        context.Users.Add(CreateUser());
        await context.SaveChangesAsync();

        var repo = new UserRepository(context);
        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyUser()
    {
        var context = GetInMemoryDbContext();
        var user = CreateUser();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        user.Name = "Updated Name";
        var repo = new UserRepository(context);
        var updated = await repo.UpdateAsync(user);

        Assert.Equal("Updated Name", updated.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        var context = GetInMemoryDbContext();
        var user = CreateUser();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(context);
        var result = await repo.DeleteAsync(user.Id);

        Assert.True(result);
        Assert.Null(await context.Users.FindAsync(user.Id));
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfNotFound()
    {
        var context = GetInMemoryDbContext();
        var repo = new UserRepository(context);

        var result = await repo.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }
}
