using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Common.Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(value, out int userId))
            {
                return userId;
            }
            throw new InvalidOperationException(
                "User ID is missing or not a valid integer in the current token."
            );
        }
    }
}
