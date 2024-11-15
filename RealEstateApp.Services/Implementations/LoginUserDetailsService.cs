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
    public int GetCurrentUserID()
    {
        try
        {
            var userIDClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userID");
            if (userIDClaim == null || string.IsNullOrEmpty(userIDClaim.Value))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            return int.Parse(userIDClaim.Value);
        }
        catch (FormatException ex)
        {
            _logger.LogError($"Invalid user ID format in token: {ex.Message}");
            throw new UnauthorizedAccessException("Invalid user authentication token");
        }
    }

    public string GetCurrentUserName()
    {
        var usernameClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name);
        if (usernameClaim == null || string.IsNullOrEmpty(usernameClaim.Value))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        return usernameClaim.Value;
    }
}
