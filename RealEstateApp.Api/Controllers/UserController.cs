using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Api.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromForm] RegisterDTO user)
    {
        Response result = await _userService.Register(user);
        return StatusCode(result.StatusCode, result.Value);
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromForm] LoginDTO user)
    {
        Response result = await _userService.Login(user);
        return StatusCode(result.StatusCode, result.Value);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("/api/Admin/AllUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
    {
        Response result = await _userService.GetAllUsers();
        return StatusCode(result.StatusCode, result.Value);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("/api/Admin/DeActivateUser")]
    public async Task<IActionResult> DeactivateUser(int userID){
        Response result = await _userService.DeactivateUser(userID);
        return StatusCode(result.StatusCode, result.Value);
    }   

    [Authorize(Roles = "Admin")]
    [HttpPost("/api/Admin/ActivateUser")]
    public async Task<IActionResult> ActivateUser(int userID){
        Response result = await _userService.ActivateUser(userID);
        return StatusCode(result.StatusCode, result.Value);
    }  
}