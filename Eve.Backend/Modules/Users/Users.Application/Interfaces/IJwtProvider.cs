using Domain.Entities;

namespace Users.Application.Interfaces
{
    public interface IJwtProvider
    {
        /// <summary>
        /// Generates a new access token for login purposes
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sessionId"></param>
        /// <returns>an access token to be used for login</returns>
        string GenerateAccessToken(User user, string sessionId);
    }
}
