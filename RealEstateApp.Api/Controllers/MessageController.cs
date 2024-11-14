using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestDTO message)
    {
        Response result = await _messageService.SendMessageAsync(message);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("between")]
    public async Task<IActionResult> GetMessagesBetweenUsers(int receiverID)
    {
        Response result = await _messageService.GetMessagesBetweenUsersAsync(receiverID);
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

    [HttpDelete("{messageID}")]
    public async Task<IActionResult> DeleteMessage(int messageID)
    {
        Response result = await _messageService.DeleteMessageAsync(messageID);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpDelete("between")]
    public async Task<IActionResult> DeleteAllMessagesBetweenUsers(int userID)
    {
        Response result = await _messageService.DeleteAllMessagesBetweenUsersAsync(userID);
        return StatusCode(result.StatusCode,result.Value);

    }
}