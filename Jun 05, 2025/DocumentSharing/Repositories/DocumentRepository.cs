using System;
using DocumentSharing.Interfaces;
using Microsoft.EntityFrameworkCore;
using DocumentSharing.Contexts;
using DocumentSharing.Models;

namespace DocumentSharing.Repositories
{
    public class DocumentRepository : IRepository<int, Document>
    {
        private readonly NotifyContext _context;

        public DocumentRepository(NotifyContext context)
        {
            _context = context;
        }

        public async Task<Document> GetById(int id)
        {
            return await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Document>> GetAll()
        {
            return await _context.Documents.ToListAsync();
        }

        public async Task<Document> Add(Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<Document> Update(int id, Document document)
        {
            var existingDocument = await GetById(id);
            if (existingDocument == null)
            {
                throw new KeyNotFoundException("Document not found");
            }

            existingDocument.FileName = document.FileName;
            existingDocument.ContentType = document.ContentType;
            existingDocument.FilePath = document.FilePath;
            existingDocument.FileData = document.FileData;

            await _context.SaveChangesAsync();
            return existingDocument;
        }

        public async Task<bool> Delete(int id)
        {
            var document = await GetById(id);
            if (document == null)
            {
                throw new KeyNotFoundException("Document not found");
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}