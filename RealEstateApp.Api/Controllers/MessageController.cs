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

    [HttpPost("sendTo")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDTO message)
    {
        Response result = await _messageService.SendMessage(message);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("betweenUsers")]
    public async Task<IActionResult> GetMessagesBetweenUsers(int userID)
    {
        var result = await _messageService.GetMessagesBetweenUsers(userID);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpPost("{messageID}/read")]
    public async Task<IActionResult> MarkMessageAsRead(int messageID)
    {
        Response result = await _messageService.MarkMessageAsRead(messageID);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("unread/count")]
    public async Task<IActionResult> GetUnreadMessagesCount()
    {
        Response result = await _messageService.GetUnreadMessagesCount();
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetAllUnreadMessages()
    {
        Response result = await _messageService.GetAllUnreadMessages();
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpDelete("DeleteForEveryone/{messageID}")]
    public async Task<IActionResult> DeleteMessageForEveryone(int messageID)
    {
        var result = await _messageService.DeleteMessageForEveryone(messageID);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpDelete("DeleteForMe/{messageID}")]
    public async Task<IActionResult> DeleteMessageForMe(int messageID)
    {
        var result = await _messageService.DeleteMessageForMe(messageID);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpDelete("betweenUsers")]
    public async Task<IActionResult> DeleteAllMessagesBetweenUsers(int userID)
    {
        Response result = await _messageService.DeleteAllMessagesBetweenUsers(userID);
        return StatusCode(result.StatusCode,result.Value);

    }
}