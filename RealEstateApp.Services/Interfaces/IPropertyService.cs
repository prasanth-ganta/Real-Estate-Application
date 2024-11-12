using System;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Services.Interfaces;

public interface IPropertyService
{
    public Task<Response> CreateProperty(PropertyDTO property);
    public Task<Response> GetOwnedProperties();
}
