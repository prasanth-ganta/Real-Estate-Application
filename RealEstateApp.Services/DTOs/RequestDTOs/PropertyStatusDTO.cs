using System;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Services.DTOs.RequestDTOs;

public class PropertyStatusDTO
{
    public PropertyListingTypeEnum PropertyStatus { get; set; }
}
