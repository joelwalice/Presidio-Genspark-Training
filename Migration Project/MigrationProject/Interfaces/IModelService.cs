using MigrationProject.DTOs;

namespace MigrationProject.Interfaces;

public interface IModelService
{
    Task<IEnumerable<ModelDto>> GetAllAsync();
    Task<ModelDto?> GetByIdAsync(int id);
    Task<ModelDto> CreateAsync(ModelCreateDto dto);
    Task<bool> UpdateAsync(int id, ModelUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}