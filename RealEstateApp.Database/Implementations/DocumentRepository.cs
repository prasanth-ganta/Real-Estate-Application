using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;

namespace RealEstateApp.Database.Implementations
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly RealEstateDbContext _realEstateDbContext;

        public DocumentRepository(RealEstateDbContext realEstateDbContext)
        {
            _realEstateDbContext = realEstateDbContext;
        }

        public async Task<bool> AddDocument(Document document)
        {
            try
            {
                await _realEstateDbContext.Documents.AddAsync(document);
                await _realEstateDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add document to database.", ex);
            }
        }

        public async Task<string> DeleteDocument(int documentID, int userID)
        {
            try
            {
                var document = await _realEstateDbContext.Documents
                    .Include(d => d.Property)
                    .FirstOrDefaultAsync(d => d.ID == documentID);

                if (document == null || document.Property.OwnerID != userID)
                {
                    throw new DocumentNotFoundException(documentID);
                }

                string fileName = document.FileName;
                _realEstateDbContext.Documents.Remove(document);
                await _realEstateDbContext.SaveChangesAsync();
                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete document {documentID}.", ex);
            }
        }
    }

    internal class DocumentNotFoundException : Exception
    {
        private int documentID;
        public DocumentNotFoundException(int documentID)
        {
            this.documentID = documentID;
        }
    }

}