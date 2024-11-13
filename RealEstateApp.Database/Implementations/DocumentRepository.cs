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

    public Task<bool> DeleteDocument(int documentId, int propertyId)
    {
        throw new NotImplementedException();
    }
}
