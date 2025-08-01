using System.ComponentModel.DataAnnotations;

namespace MigrationProject.DTOs;

public class ColorUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
