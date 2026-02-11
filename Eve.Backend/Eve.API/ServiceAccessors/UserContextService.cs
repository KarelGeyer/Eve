using Users.Application.Interfaces;

namespace Eve.API.ServiceAccessors
{
    public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
    {
        public string GetIpAddress()
        {
            var context = httpContextAccessor.HttpContext;
            var ip =
                context?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context?.Connection.RemoteIpAddress?.ToString();
            return ip ?? "Unknown";
        }

        public string GetUserAgent() => httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString() ?? "Unknown";
    }
}
