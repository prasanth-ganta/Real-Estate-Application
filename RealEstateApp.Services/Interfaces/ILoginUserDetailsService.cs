namespace RealEstateApp.Services.Interfaces;

public interface ILoginUserDetailsService
{
    public int GetCurrentUserID();
    public string GetCurrentUserName();
}
