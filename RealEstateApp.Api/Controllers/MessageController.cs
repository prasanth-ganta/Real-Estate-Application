using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Api.Controllers;

[Authorize(Roles = "User,Admin")]
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDTO message)
    {
        Response result = await _messageService.SendMessageAsync(message);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("between")]
    public async Task<IActionResult> GetMessagesBetweenUsers(int userId)
    {
        var result = await _messageService.GetMessagesBetweenUsersAsync(userId);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpPost("{messageID}/read")]
    public async Task<IActionResult> MarkMessageAsRead(int messageID)
    {
        Response result = await _messageService.MarkMessageAsReadAsync(messageID);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("unread/count")]
    public async Task<IActionResult> GetUnreadMessagesCount()
    {
        Response result = await _messageService.GetUnreadMessagesCountAsync();
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetAllUnreadMessages()
    {
        Response result = await _messageService.GetAllUnreadMessagesAsync();
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpDelete("DeleteForEveryone/{messageId}")]
    public async Task<IActionResult> DeleteMessageForEveryone(int messageId)
    {
        var result = await _messageService.DeleteMessageForEveryoneAsync(messageId);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpDelete("DeleteForMe/{messageId}")]
    public async Task<IActionResult> DeleteMessageForMe(int messageId)
    {
        var result = await _messageService.DeleteMessageForMe(messageId);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpDelete("between")]
    public async Task<IActionResult> DeleteAllMessagesBetweenUsers(int userID)
    {
        Response result = await _messageService.DeleteAllMessagesBetweenUsersAsync(userID);
        return StatusCode(result.StatusCode,result.Value);

    }
}