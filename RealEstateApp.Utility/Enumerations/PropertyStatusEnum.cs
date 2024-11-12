using System.Text.Json.Serialization;

namespace RealEstateApp.Utility.Enumerations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyStatusEnum
{
    Rental = 1,
    Sell,
    Unavailable
}
