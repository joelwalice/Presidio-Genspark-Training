using DocumentSharing.Models;

namespace DocumentSharing.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> GetByIdAsync(int id);
        Task<IEnumerable<Document>> GetAllAsync();
        Task<IEnumerable<Document>> GetByUserIdAsync(int userId);
        Task<Document> CreateAsync(Document document);
        Task<Document> UpdateAsync(Document document);
        Task DeleteAsync(int id);
    }
}
