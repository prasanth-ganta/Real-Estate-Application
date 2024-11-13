using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Services.Interfaces; 

public interface IDocumentService
{
    public Task<Response> AddDocument(DocumentDTO document, int propertyID);
    public Task<Response> DeleteDocument(int documentID);
}

