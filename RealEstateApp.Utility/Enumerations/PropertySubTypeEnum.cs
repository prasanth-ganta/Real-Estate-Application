using System.Text.Json.Serialization;

namespace RealEstateApp.Utility.Enumerations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertySubTypeEnum
{
    BHK1 = 1,
    BHK2,
    BHK3,
    BHK4,
    Office,
    Retail,
    Industrial,
    VacantLand,
    AgricultureLand,
    RecreationalLand,
    Hotel,
    Hospital,
    School,
    OldAgeHome
}
