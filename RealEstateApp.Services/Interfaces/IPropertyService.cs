using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.ResponseType;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Services.Interfaces;

public interface IPropertyService
{
    public Task<Response> CreateProperty(PropertyDTO property);
    public Task<Response> GetAllProperties(RetrivalOptionsEnum retivalOption);
    public Task<Response> GetOwnedProperties(RetrivalOptionsEnum retivalOption);
    public Task<Response> GetAllPendingProperties();
    public Task<Response> SoftDeleteProperty(int id);
}
