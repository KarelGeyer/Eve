using System.Security.Claims;

namespace Common.Shared.Extensions
{
    //todo: use everywhere userId is needed and is available
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(value, out int userId))
            {
                return userId;
            }
            throw new InvalidOperationException("User ID is missing or not a valid integer in the current token.");
        }
    }
}
