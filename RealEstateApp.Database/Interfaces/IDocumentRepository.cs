using RealEstateApp.Database.Entities;
namespace RealEstateApp.Database.Interfaces;

public interface IDocumentRepository
{
    Task<bool> AddDocument(Document document);
    Task<string> DeleteDocument(int documentID , int userID);
}
