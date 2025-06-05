using System;
using DocumentSharing.Interfaces;
using DocumentSharing.Models;
using DocumentSharing.Contexts;
using DocumentSharing.Repositories;
using System.Collections.Generic;

namespace DocumentSharing.Services
{
    public class DocumentService : IDocumentServices
    {
        private readonly IRepository<int, Document> _documentRepository;

        public DocumentService(IRepository<int, Document> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _documentRepository.GetById(id);
        }

        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            return await _documentRepository.GetAll();
        }

        public async Task<Document> AddDocumentAsync(Document document)
        {
            return await _documentRepository.Add(document);
        }

        public async Task<Document> UpdateDocumentAsync(int id, Document document)
        {
            return await _documentRepository.Update(id, document);
        }

        public async Task<bool> DeleteDocumentAsync(int id)
        {
            return await _documentRepository.Delete(id);
        }
    }
}