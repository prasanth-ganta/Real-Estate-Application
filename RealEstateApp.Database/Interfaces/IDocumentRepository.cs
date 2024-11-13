using RealEstateApp.Database.Entities;
namespace RealEstateApp.Database.Interfaces;

public interface IDocumentRepository
{
    Task<bool> AddDocument(Document document);
    Task<bool> DeleteDocument(int documentId,int propertyId);
}
