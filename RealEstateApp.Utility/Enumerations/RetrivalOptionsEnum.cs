using System.Text.Json.Serialization;

namespace RealEstateApp.Utility.Enumerations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RetrivalOptionsEnum
{
    Rental = 1,
    Sale,
    All
}
