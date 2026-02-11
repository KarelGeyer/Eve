using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Users.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(User user, string sessionId);
    }
}
