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

    public async Task<Response> GetMessagesBetweenUsers(int receiverID)
    {
        int senderID = _loginUserDetailsService.GetCurrentUserID();

        List<Message> messages = await _messageRepository.GetMessagesBetweenUsers(senderID,receiverID);
        if(messages == null)
        {
            return new Response(404, "No Content Found");
        }
        List<MessageResponseDTO> responseMessages = _mapper.Map<List<MessageResponseDTO>>(messages); 

        return new Response(200,responseMessages);
    }

    public async Task<Response> SendMessage(MessageDTO sendMessage)
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
        
        bool isSent = await _messageRepository.SendMessage(message,senderName);
        if(isSent) return new Response(200,"Message Sent Successfully");
        return new Response(400,"Invalid Receiver ID");
    }

    public async Task<Response> MarkMessageAsRead(int messageID)
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            bool isMarked = await _messageRepository.MarkMessageAsRead(messageID, userID);
            
            return isMarked 
                ? new Response(200, "Successfully Changed to Read")
                : new Response(400, "No such Messages Exist Or Unauthorized User");
        }
        catch (UnauthorizedAccessException)
        {
            return new Response(400, "User ID not found in claims");
        }
    }

    public async Task<Response> GetUnreadMessagesCount()
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            int count = await _messageRepository.GetUnreadMessagesCount(userID);
            
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

    public async Task<Response> GetAllUnreadMessages()
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            List<Message> messages = await _messageRepository.GetAllUnreadMessages(userID);
            
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

   public async Task<Response> DeleteMessageForEveryone(int messageID)
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            bool result = await _messageRepository.DeleteMessageForEverone(messageID, userID);
            if (result)
            {
                return new Response(200, "Message deleted successfully.");
            }
            return new Response(400, "No Message to delete");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response> DeleteAllMessagesBetweenUsers(int userID)
    {
        int currentUserID = _loginUserDetailsService.GetCurrentUserID();
        bool result = await _messageRepository.DeleteAllMessagesBetweenUsers(currentUserID, userID);

        return result
            ? new Response(200, "All messages between the users have been deleted successfully.")
            : new Response(404, "No messages found between the users to delete.");
    }

    public async Task<Response> DeleteMessageForMe(int messageID)
    {
        int userID = _loginUserDetailsService.GetCurrentUserID();
        bool result = await _messageRepository.DeleteMessageForMe(messageID, userID);
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
