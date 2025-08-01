using MigrationProject.DTOs;

namespace MigrationProject.Interfaces;

public interface IContactUsService
{
    Task<IEnumerable<ContactUsReadDto>> GetAllAsync();
    Task<ContactUsReadDto?> GetByIdAsync(int id);
    Task<ContactUsReadDto> CreateAsync(ContactUsCreateDto dto);
    Task<bool> DeleteAsync(int id);
}