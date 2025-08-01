using System.ComponentModel.DataAnnotations;

namespace MigrationProject.Models;

public class Model
{
    [Key]
    public int ModelId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ModelName { get; set; } = string.Empty; 

    public ICollection<Product> Products { get; set; } = new List<Product>();
}