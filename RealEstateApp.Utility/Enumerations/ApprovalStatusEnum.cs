using System.Text.Json.Serialization;

namespace RealEstateApp.Utility.Enumerations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApprovalStatusEnum
{
    Pending = 1,
    Approved
}