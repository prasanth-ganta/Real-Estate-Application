using System;

namespace RealEstateApp.Services.DTOs.ResponseDTOs;

public class MessageResponseDTO
{
    public string senderName { get; set; }
    public string receiverName { get; set; }
    public string content { get; set; }
}
