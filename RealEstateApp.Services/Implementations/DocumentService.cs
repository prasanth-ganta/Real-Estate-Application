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
            
            List<string> validExtensions = new List<string>(){ ".jpg", ".png", "pdf" };

            string documentExtension = Path.GetExtension(document.uploadDocument.FileName);
            if(!validExtensions.Contains(documentExtension))
            {
                new Response(400,"Invalid File Type");
            }
            long documentSize = document.uploadDocument.Length;
            if(documentSize > 5*1024*1024)
            {
                new Response(400," Max doc size can be 5MB ");
            }
            string fileName = Guid.NewGuid().ToString()+documentExtension;
            string path = _configuration["UploadedFiles:Path"];
            using (FileStream stream = new FileStream(Path.Combine(path,fileName),FileMode.Create))
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

    public async Task<Response> DeleteDocument(int documentId, int propertyId)
    {
        try
        {
            if (documentId <= 0)
            {
                throw new ArgumentException("Invalid document ID");
            }

            if (propertyId <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            var currentUserId = _loginUserDetailsService.GetCurrentUserId();

            var deleted = await _documentRepository.DeleteDocument(documentId, propertyId);
            if (!deleted)
            {
                throw new KeyNotFoundException(
                    $"Document with ID {documentId} not found for property {propertyId}"
                );
            }

            _logger.LogInformation(
                $"Successfully deleted document {documentId} from property {propertyId}"
            );
            return new Response(200, "Document deleted successfully");
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid argument while deleting document: {ex.Message}");
            throw;
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Unauthorized document deletion attempt: {ex.Message}");
            throw;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Document not found: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Error deleting document {documentId} from property {propertyId}: {ex.Message}"
            );
            throw new InvalidOperationException($"Failed to delete document", ex);
        }
    }
}