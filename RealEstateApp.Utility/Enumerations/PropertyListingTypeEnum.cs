using System;
using System.Text.Json.Serialization;

namespace RealEstateApp.Utility.Enumerations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyListingTypeEnum
{
    Rental = 1,
    Sale,
    All

}
