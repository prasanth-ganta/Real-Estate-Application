using RealEstateApp.Database.Entities;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.ResponseType; 

namespace RealEstateApp.Services.Interfaces;

public interface IMessageService
{
    public Task<Response> SendMessage(MessageDTO sendMessage);
    public Task<Response> GetMessagesBetweenUsers(int receiverID);
    public Task<Response> MarkMessageAsRead(int messageID);
    public Task<Response> GetUnreadMessagesCount();
    public Task<Response> GetAllUnreadMessages();
    public Task<Response> DeleteMessageForEveryone(int messageID);
    public Task<Response> DeleteAllMessagesBetweenUsers(int userID);
    public Task<Response> DeleteMessageForMe(int messageID);
}