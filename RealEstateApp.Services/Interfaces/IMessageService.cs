using RealEstateApp.Database.Entities;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.ResponseType; 

namespace RealEstateApp.Services.Interfaces;

public interface IMessageService
{
    public Task<Response> SendMessageAsync(SendMessageRequestDTO sendMessage);
    public Task<Response> GetMessagesBetweenUsersAsync(int receiverID);
    public Task<Response> MarkMessageAsReadAsync(int messageID);
    public Task<Response> GetUnreadMessagesCountAsync();
    public Task<Response> GetAllUnreadMessagesAsync();
    public Task<Response> DeleteMessageAsync(int messageID);
    public Task<Response> DeleteAllMessagesBetweenUsersAsync(int userID);
}