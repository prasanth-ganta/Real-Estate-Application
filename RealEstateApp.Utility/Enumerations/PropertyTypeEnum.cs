using System.Text.Json.Serialization;

namespace RealEstateApp.Utility.Enumerations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyTypeEnum
{
    Residential = 1,
    Commercial,
    Land,
    SpecialPurpose,
    Luxuary
}
