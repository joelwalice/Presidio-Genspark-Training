namespace MigrationProject.DTOs;

public class ContactUsCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}