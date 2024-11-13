using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;

namespace RealEstateApp.Database.Implementations;

public class DocumentRepository : IDocumentRepository
{
    private readonly RealEstateDbContext _realEstateDbContext;
    public DocumentRepository(RealEstateDbContext realEstateDbContext)
    {
        _realEstateDbContext = realEstateDbContext;
    }
    public async Task<bool> AddDocument(Document document)
    {
        _realEstateDbContext.Documents.AddAsync(document);
        await _realEstateDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<string> DeleteDocument(int documentID , int userID)
    {
        var document = await _realEstateDbContext.Documents
            .Include(d => d.Property)
            .FirstOrDefaultAsync(d => d.ID == documentID);

        if (document == null || document.Property.OwnerID != userID)
        {
            throw new KeyNotFoundException("Document not found ");
        }
        string fileName = document.FileName;
        _realEstateDbContext.Documents.Remove(document);
        await _realEstateDbContext.SaveChangesAsync();

        return fileName; 

    }


}
