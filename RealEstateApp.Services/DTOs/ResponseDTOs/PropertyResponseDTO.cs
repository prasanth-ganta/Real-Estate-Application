using RealEstateApp.Services.DTOs.RequestDTOs;

namespace RealEstateApp.Services.DTOs.ResponseDTOs;

public class PropertyResponseDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string PropertyType { get; set; }
    public string PropertySubType { get; set; }
    public string ApprovalStatus { get; set; }
    public string OwnerName { get; set; }
    public LocationDTO Location { get; set; }
    public string PropertyStatus { get; set; }
    public List<DocumentDTO> documents  { get; set; }
}
