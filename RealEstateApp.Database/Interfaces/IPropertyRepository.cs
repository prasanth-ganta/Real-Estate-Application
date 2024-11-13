using RealEstateApp.Database.Entities;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Database.Interfaces;

public interface IPropertyRepository
{
    public Task AddProperty(Property newProperty, string username);
    public Task<List<Property>> GetAllProperties(RetrivalOptionsEnum propertyStatus);
    public Task<List<Property>> GetOwnedProperties(int ownerId, RetrivalOptionsEnum retivalOption);
    public Task<List<Property>> GetAllPendingProperties();
    public Task<bool> SoftDeleteProperty(int id, string value);
}
