namespace RealEstateApp.Services.DTOs.ResponseDTOs;

public class MessageResponseDTO
{
    public string SenderName { get; set; }
    public string ReceiverName { get; set; }
    public string Content { get; set; }
    public DateTime TimeStamp { get; set; }
}
