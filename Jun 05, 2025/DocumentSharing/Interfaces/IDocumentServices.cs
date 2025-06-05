using System;
using DocumentSharing.Models;

namespace DocumentSharing.Interfaces
{
    public interface IDocumentServices
    {
        Task<Document> GetDocumentByIdAsync(int id);
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<Document> AddDocumentAsync(Document document);
        Task<Document> UpdateDocumentAsync(int id, Document document);
        Task<bool> DeleteDocumentAsync(int id);
    }
}