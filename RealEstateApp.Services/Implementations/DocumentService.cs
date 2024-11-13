using System.Windows.Markup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
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
    public DocumentService(IDocumentRepository documentRepository, ILoginUserDetailsService loginUserDetailsService, ILogger<DocumentService> logger, IConfiguration configuration)
    {
        _documentRepository = documentRepository;
        _loginUserDetailsService = loginUserDetailsService;
        _logger = logger;
        _configuration = configuration;
    }
    public async Task<Response> AddDocument(DocumentDTO document, int propertyId)
    {
        try
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var currentUserId = _loginUserDetailsService.GetCurrentUserId();

            List<string> validExtensions = new List<string>() { ".jpg", ".png", "pdf" };

            string documentExtension = Path.GetExtension(document.uploadDocument.FileName);
            if (!validExtensions.Contains(documentExtension))
            {
                new Response(400, "Invalid File Type");
            }
            long documentSize = document.uploadDocument.Length;
            if (documentSize > 5 * 1024 * 1024)
            {
                new Response(400, " Max doc size can be 5MB ");
            }
            string fileName = Guid.NewGuid().ToString() + documentExtension;
            string path = Path.Combine(_configuration["UploadedFiles:Path"], "UploadedDocuments");
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                document.uploadDocument.CopyTo(stream);
            }

            Document configuredDocument = new Document
            {
                FileName = fileName,
                Description = document.Description,
                PropertyId = propertyId
            };

            await _documentRepository.AddDocument(configuredDocument);
            return new Response(201, "Document added successfully");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Unauthorized document addition attempt: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding document to property {propertyId}: {ex.Message}");
            throw new InvalidOperationException("Failed to add document to property", ex);
        }
    }

    public async Task<Response> DeleteDocument(int documentId)
    {
        var currentUserId = _loginUserDetailsService.GetCurrentUserId();

        var fileName = await _documentRepository.DeleteDocument(documentId,currentUserId);
        string path = Path.Combine(_configuration["UploadedFiles:Path"], "UploadedDocuments", fileName);
        
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            _logger.LogWarning($"Physical file not found for document {documentId}: {path}");
        }

        return new Response(200, "Document deleted successfully");

    }
}
