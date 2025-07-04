using JobPortalAPI.Contexts;
using JobPortalAPI.DTOs;
using JobPortalAPI.Models;
using JobPortalAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class FileServiceTests
{
    private JobContexts GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<JobContexts>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // fresh DB for each test
            .Options;

        return new JobContexts(options);
    }

    private IFormFile CreateMockFormFile(string content, string fileName, string contentType)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        return new FormFile(stream, 0, bytes.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    [Fact]
    public async Task UploadAsync_ShouldStoreFileAndReturnFileName()
    {
        // Arrange
        var db = GetInMemoryDbContext();
        var service = new FileService(db);

        var jobSeeker = new JobSeeker
        {
            Id = Guid.NewGuid(),
            Name = "Test Seeker",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        db.JobSeekers.Add(jobSeeker);
        await db.SaveChangesAsync();

        var file = CreateMockFormFile("Test resume content", "resume.pdf", "application/pdf");
        var dto = new ResumeUploadDto
        {
            File = file,
            JobSeekerId = jobSeeker.Id
        };

        // Act
        var resultFileName = await service.UploadAsync(dto);

        // Assert
        var storedResume = await db.ResumeDocuments.FirstOrDefaultAsync();
        var updatedSeeker = await db.JobSeekers.FindAsync(jobSeeker.Id);

        Assert.Equal("resume.pdf", resultFileName);
        Assert.NotNull(storedResume);
        Assert.Equal(storedResume.Id, updatedSeeker.ResumeId);
    }

    [Fact]
    public async Task UploadAsync_ShouldThrowException_WhenJobSeekerNotFound()
    {
        // Arrange
        var db = GetInMemoryDbContext();
        var service = new FileService(db);

        var file = CreateMockFormFile("Test", "test.docx", "application/msword");
        var dto = new ResumeUploadDto
        {
            File = file,
            JobSeekerId = Guid.NewGuid() // non-existent
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.UploadAsync(dto));
    }

    [Fact]
    public async Task DownloadAsync_ShouldReturnFileContentResult_IfFileExists()
    {
        // Arrange
        var db = GetInMemoryDbContext();
        var service = new FileService(db);

        var resume = new ResumeDocument
        {
            Id = Guid.NewGuid(),
            FileName = "test.pdf",
            FileType = "application/pdf",
            Content = Encoding.UTF8.GetBytes("PDF Content"),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        db.ResumeDocuments.Add(resume);
        await db.SaveChangesAsync();

        // Act
        var result = await service.DownloadAsync("test.pdf");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<FileContentResult>(result);
        Assert.Equal("test.pdf", result.FileDownloadName);
        Assert.Equal("application/pdf", result.ContentType);
    }

    [Fact]
    public async Task DownloadAsync_ShouldReturnNull_IfFileNotFound()
    {
        // Arrange
        var db = GetInMemoryDbContext();
        var service = new FileService(db);

        // Act
        var result = await service.DownloadAsync("nonexistent.pdf");

        // Assert
        Assert.Null(result);
    }
}
