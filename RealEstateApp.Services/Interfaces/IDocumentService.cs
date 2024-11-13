using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Services.Interfaces;

public interface IDocumentService
{
    public Task<Response> AddDocument(DocumentDTO document, int propertyId);
    public Task<Response> DeleteDocument(int documentId);
}
