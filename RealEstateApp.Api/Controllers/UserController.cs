using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;

namespace RealEstateApp.Api.Controllers
{
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
        public async Task<IActionResult> Register([FromForm] RegisterDTO user)
        {
            var result = await _userService.Register(user);
            return StatusCode(result.StatusCode,result.Value);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO user)
        {
            var result = await _userService.Login(user);
            return StatusCode(result.StatusCode,result.Value);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("AllUsers")]
        public IActionResult GetAllUsers()
        {
            var result =  _userService.GetAllUsers();
            return StatusCode(result.StatusCode,result.Value);
        }
    }
}
