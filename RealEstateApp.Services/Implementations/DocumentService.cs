using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Services.Implementations;
public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly ILoginUserDetailsService _loginUserDetailsService;
    private readonly ILogger<DocumentService> _logger;
    private readonly IConfiguration _configuration;
    private const int MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

    public DocumentService(IDocumentRepository documentRepository,
        ILoginUserDetailsService loginUserDetailsService,
        ILogger<DocumentService> logger,
        IConfiguration configuration)
    {
        _documentRepository = documentRepository;
        _loginUserDetailsService = loginUserDetailsService;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Response> AddDocument(DocumentDTO document, int propertyID)
    {
        try
        {
            if (document == null)
            {
                throw new Exception("Document cannot be null");
            }

            ValidateDocument(document);

            string fileName = await SaveDocumentFile(document);

            Document configuredDocument = new Document
            {
                FileName = fileName,
                Description = document.Description,
                PropertyID = propertyID
            };

            await _documentRepository.AddDocument(configuredDocument);
            return new Response(201, "Document added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to process document: {ex.Message}", ex);
            return new Response(500, "Failed to process document");
        }
    }

    public async Task<Response> DeleteDocument(int documentID)
    {
        try
        {
            int currentUserID = _loginUserDetailsService.GetCurrentUserID();
            string fileName = await _documentRepository.DeleteDocument(documentID, currentUserID);
            string path = Path.Combine(_configuration["UploadedFiles:Path"], "UploadedDocuments", fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                _logger.LogWarning($"Physical file not found: {path}");
            }
            return new Response(200, "Document deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to delete document: {ex.Message}", ex);
            return new Response(500, "Failed to delete document");
        }
    }

    private void ValidateDocument(DocumentDTO document)
    {
        List<string> validExtensions = new List<string>() { ".jpg", ".png", ".pdf" };
        string documentExtension = Path.GetExtension(document.uploadDocument.FileName).ToLower();

        if (!validExtensions.Contains(documentExtension))
        {
            throw new Exception($"Invalid file type: {documentExtension}");
        }

        if (document.uploadDocument.Length > MaxFileSizeInBytes)
        {
            throw new Exception($"File size exceeds maximum limit of 5MB");
        }
    }

    private async Task<string> SaveDocumentFile(DocumentDTO document)
    {
        try
        {
            string documentExtension = Path.GetExtension(document.uploadDocument.FileName);
            string fileName = Guid.NewGuid().ToString() + documentExtension;
            string path = Path.Combine(_configuration["UploadedFiles:Path"], "UploadedDocuments", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await document.uploadDocument.CopyToAsync(stream);
            }

            return fileName;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

}