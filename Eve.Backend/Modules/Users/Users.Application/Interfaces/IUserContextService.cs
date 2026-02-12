namespace Users.Application.Interfaces
{
    public interface IUserContextService
    {
        string GetIpAddress();
        string GetUserAgent();
    }
}
