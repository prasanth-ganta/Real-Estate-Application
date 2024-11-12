using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Interfaces;

public interface IPropertyRepository
{
    Task AddProperty(Property newProperty);
    Task<List<Property>> GetOwnedProperties(int ownerId);
}
