using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Services.Interfaces;

namespace RealEstateApp.Services.Implementations;

public class CurrentUser  : ICurrentUser 
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser (IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value;
    }
}
