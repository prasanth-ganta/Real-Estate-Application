using RealEstateApp.Database.Entities;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Services.Interfaces;

public interface IMessageService
{
    public Task<Response> SendMessageAsync(SendMessageRequestDTO sendMessage);
    public Task<Response> GetMessagesBetweenUsersAsync(int receiverId);
    public Task<Response> MarkMessageAsReadAsync(int messageId);
    public Task<Response> GetUnreadMessagesCountAsync();
    public Task<Response> GetAllUnreadMessagesAsync();
    public Task<Response> DeleteMessageAsync(int messageId);
    public Task<Response> DeleteAllMessagesBetweenUsersAsync(int userId);
}