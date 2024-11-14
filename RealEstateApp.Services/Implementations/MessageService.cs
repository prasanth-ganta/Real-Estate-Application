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
        var userIDClaim = _httpContext.HttpContext.User.FindFirst("userID");
        if (userIDClaim == null)
        {
            return new Response(400, "User ID not found in claims");
        } 
        int senderID = int.Parse(userIDClaim.Value);

        List<Message> messages = await _messageRepository.GetMessagesBetweenUsersAsync(senderID,receiverID);
        if(messages == null)
        {
            return new Response(404, "No Content Found");
        }
        List<MessageResponseDTO> responseMessages = _mapper.Map<List<MessageResponseDTO>>(messages); 

        return new Response(200,responseMessages);
    }

    public async Task<Response> SendMessageAsync(MessageDTO sendMessage)
    {
        var userIDClaim = _httpContext.HttpContext.User.FindFirst("userID");
        var userNameClaim = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name);
        if (userIDClaim == null)
        {
            return new Response(400, "User ID not found in claims");
        }
        int senderID = int.Parse(userIDClaim.Value);
        Message message = new Message
        {
            SenderID = senderID,
            ReceiverID = sendMessage.ReceiverID,
            Chat = sendMessage.Content,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };
        
        bool isSent = await _messageRepository.SendMessageAsync(message,userNameClaim.Value);
        if(isSent) return new Response(200,"Message Sent Successfully");
        return new Response(400,"Invalid Receiver ID");
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

   public async Task<Response> DeleteMessageForEveryoneAsync(int messageId)
    {
        int userId = int.Parse(_httpContext.HttpContext.User.FindFirst("userId").Value);
        bool result = await _messageRepository.DeleteMessageForEveroneAsync(messageId, userId);
        if (result)
        {
            return new Response(200, "Message deleted successfully.");
        }
        catch (UnauthorizedAccessException)
        {
            return new Response(400, "User ID not found in claims");
        }
    }

    public async Task<Response> DeleteAllMessagesBetweenUsersAsync(int userID)
    {
        int currentUserId = int.Parse(_httpContext.HttpContext.User.FindFirst("userId").Value);
        bool result = await _messageRepository.DeleteAllMessagesBetweenUsersAsync(currentUserId, userId);

            return result
                ? new Response(200, "All messages between the users have been deleted successfully.")
                : new Response(404, "No messages found between the users to delete.");
        }
    }

    public async Task<Response> DeleteMessageForMe(int messageId)
    {
        int userId = int.Parse(_httpContext.HttpContext.User.FindFirst("userId").Value);
        bool result = await _messageRepository.DeleteMessageForMe(messageId, userId);
        if (result)
        {
            return new Response(200, "Message deleted successfully.");
        }
        else
        {
            return new Response(404, "Message not found ");
        }
    }
}
