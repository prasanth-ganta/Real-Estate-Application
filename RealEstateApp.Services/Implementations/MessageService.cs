using AutoMapper;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.DTOs.ResponseDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly ILoginUserDetailsService _loginUserDetailsService;
    private readonly IMapper _mapper;

    public MessageService(
        IMessageRepository messageRepository,
        ILoginUserDetailsService loginUserDetailsService,
        IMapper mapper)
    {
        _messageRepository = messageRepository;
        _loginUserDetailsService = loginUserDetailsService;
        _mapper = mapper;
    }

    public async Task<Response> GetMessagesBetweenUsersAsync(int receiverID)
    {
        try
        {
            int senderID = _loginUserDetailsService.GetCurrentUserID();
            List<Message> messages = await _messageRepository.GetMessagesBetweenUsersAsync(senderID, receiverID);
            
            if (messages == null)
            {
                return new Response(404, "No Content Found");
            }
            
            List<MessageResponseDTO> responseMessages = _mapper.Map<List<MessageResponseDTO>>(messages);
            return new Response(200, responseMessages);
        }
        catch (UnauthorizedAccessException)
        {
            return new Response(400, "User ID not found in claims");
        }
    }

    public async Task<Response> SendMessageAsync(SendMessageRequestDTO sendMessage)
    {
        try
        {
            int senderID = _loginUserDetailsService.GetCurrentUserID();
            string senderName = _loginUserDetailsService.GetCurrentUserName();

            Message message = new Message
            {
                SenderID = senderID,
                ReceiverID = sendMessage.ReceiverID,
                Chat = sendMessage.Content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            bool isSent = await _messageRepository.SendMessageAsync(message, senderName);
            return isSent 
                ? new Response(200, "Message Sent Successfully")
                : new Response(400, "Invalid Receiver ID");
        }
        catch (UnauthorizedAccessException)
        {
            return new Response(400, "User ID not found in claims");
        }
    }

    public async Task<Response> MarkMessageAsReadAsync(int messageID)
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            bool isMarked = await _messageRepository.MarkMessageAsReadAsync(messageID, userID);
            
            return isMarked 
                ? new Response(200, "Successfully Changed to Read")
                : new Response(400, "No such Messages Exist Or Unauthorized User");
        }
        catch (UnauthorizedAccessException)
        {
            return new Response(400, "User ID not found in claims");
        }
    }

    public async Task<Response> GetUnreadMessagesCountAsync()
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            int count = await _messageRepository.GetUnreadMessagesCountAsync(userID);
            
            return count == 0
                ? new Response(204, new { Message = "All messages are Read", UnreadMessages = count })
                : new Response(200, new { UnreadMessages = count });
        }
        catch (UnauthorizedAccessException)
        {
            return new Response(400, "User ID not found in claims");
        }
        catch (NullReferenceException)
        {
            return new Response(400, "No Such User Exist");
        }
    }

    public async Task<Response> GetAllUnreadMessagesAsync()
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            List<Message> messages = await _messageRepository.GetAllUnreadMessagesAsync(userID);
            
            if (messages == null)
            {
                return new Response(404, "No Unread Messages Found");
            }
            
            List<MessageResponseDTO> responseMessages = _mapper.Map<List<MessageResponseDTO>>(messages);
            return new Response(200, responseMessages);
        }
        catch (UnauthorizedAccessException)
        {
            return new Response(400, "User ID not found in claims");
        }
    }

    public async Task<Response> DeleteMessageAsync(int messageID)
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            bool result = await _messageRepository.DeleteMessageAsync(messageID, userID);
            
            return result
                ? new Response(200, "Message deleted successfully.")
                : new Response(404, "Message not found ");
        }
        catch (UnauthorizedAccessException)
        {
            return new Response(400, "User ID not found in claims");
        }
    }

    public async Task<Response> DeleteAllMessagesBetweenUsersAsync(int userID)
    {
        try
        {
            int currentUserID = _loginUserDetailsService.GetCurrentUserID();
            bool result = await _messageRepository.DeleteAllMessagesBetweenUsersAsync(userID, currentUserID);

            return result
                ? new Response(200, "All messages between the users have been deleted successfully.")
                : new Response(404, "No messages found between the users to delete.");
        }
        catch (UnauthorizedAccessException)
        {
            return new Response(400, "User ID not found in claims");
        }
    }
}