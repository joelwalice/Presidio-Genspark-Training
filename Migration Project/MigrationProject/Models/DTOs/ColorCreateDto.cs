using System.ComponentModel.DataAnnotations;

namespace MigrationProject.DTOs;

public class ColorCreateDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
