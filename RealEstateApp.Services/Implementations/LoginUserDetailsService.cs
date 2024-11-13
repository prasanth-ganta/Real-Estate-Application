using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RealEstateApp.Services.Interfaces;

namespace RealEstateApp.Services.Implementations;

public class LoginUserDetailsService : ILoginUserDetailsService
{
    private IHttpContextAccessor _httpContextAccessor;
    private ILogger<LoginUserDetailsService> _logger;
    public LoginUserDetailsService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public int GetCurrentUserId()
    {
        try
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            return int.Parse(userIdClaim.Value);
        }
        catch (FormatException ex)
        {
            _logger.LogError($"Invalid user ID format in token: {ex.Message}");
            throw new UnauthorizedAccessException("Invalid user authentication token");
        }
    }

    public string GetCurrentUserName()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name);
        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        return userIdClaim.Value;
    }
}
