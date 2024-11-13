using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;

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
        var result = await _messageService.SendMessageAsync(message);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("between")]
    public async Task<IActionResult> GetMessagesBetweenUsers(int receiverId)
    {
        var result = await _messageService.GetMessagesBetweenUsersAsync(receiverId);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpPost("{messageId}/read")]
    public async Task<IActionResult> MarkMessageAsRead(int messageId)
    {
        var result = await _messageService.MarkMessageAsReadAsync(messageId);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("unread/count")]
    public async Task<IActionResult> GetUnreadMessagesCount()
    {
        var result = await _messageService.GetUnreadMessagesCountAsync();
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetAllUnreadMessages()
    {
        var result = await _messageService.GetAllUnreadMessagesAsync();
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessage(int messageId)
    {
        var result = await _messageService.DeleteMessageAsync(messageId);
        return StatusCode(result.StatusCode,result.Value);
    }

    [HttpDelete("between")]
    public async Task<IActionResult> DeleteAllMessagesBetweenUsers(int userId)
    {
        var result = await _messageService.DeleteAllMessagesBetweenUsersAsync(userId);
        return StatusCode(result.StatusCode,result.Value);

    }
}