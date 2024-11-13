using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
     private readonly IHttpContextAccessor _httpContext;
     private readonly IMapper _mapper;

    public MessageService(IMessageRepository messageRepository, IHttpContextAccessor httpContext, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _httpContext = httpContext;
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

    public async Task<Response> SendMessageAsync(SendMessageRequestDTO sendMessage)
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
        if(await _messageRepository.MarkMessageAsReadAsync(messageID, int.Parse(_httpContext.HttpContext.User.FindFirst("userID").Value)))
        {
            return new Response(200,"Successfully Changed to Read");
        }
        return new Response(400,"No such Messages Exist Or Unauthorized User");
    }

    public async Task<Response> GetUnreadMessagesCountAsync()
    {
        try
        {
            int count = await _messageRepository.GetUnreadMessagesCountAsync(int.Parse(_httpContext.HttpContext.User.FindFirst("userID").Value));
            if(count == 0)
            {
                return new Response(204,new { Message = "All messages are Read", UnreadMessages = count });
            }
            return new Response(200,new { UnreadMessages = count });
        }
        catch(NullReferenceException exception)
        {
        return new Response(400,"No Such User Exist");
        }
    }

    public async Task<Response> GetAllUnreadMessagesAsync()
    {
        List<Message> messages = await _messageRepository.GetAllUnreadMessagesAsync(int.Parse(_httpContext.HttpContext.User.FindFirst("userID").Value));
        if(messages == null)
        {
            return new Response(404, "No Unread Messages Found");
        }
        List<MessageResponseDTO> responseMessages = _mapper.Map<List<MessageResponseDTO>>(messages); 

        return new Response(200,responseMessages);
    }

   public async Task<Response> DeleteMessageAsync(int messageID)
    {
        int userID = int.Parse(_httpContext.HttpContext.User.FindFirst("userID").Value);
        bool result = await _messageRepository.DeleteMessageAsync(messageID, userID);
        if (result)
        {
            return new Response(200, "Message deleted successfully.");
        }
        else
        {
            return new Response(404, "Message not found ");
        }
   }

    public async Task<Response> DeleteAllMessagesBetweenUsersAsync(int userID)
    {
        int currentUserID = int.Parse(_httpContext.HttpContext.User.FindFirst("userID").Value);
        bool result = await _messageRepository.DeleteAllMessagesBetweenUsersAsync(userID, currentUserID);

        if (result)
        {
            return new Response(200, "All messages between the users have been deleted successfully.");
        }
        else
        {
            return new Response(404, "No messages found between the users to delete.");
        }
    }
}
