using System;
using System.Collections.Generic;
using System.Text;
using Users.Application.Dtos.Requests;

namespace Users.Application.Interfaces
{
    public interface IUserRegistrationService
    {
        Task<bool> RegisterUserAsync(UserRegistrationRequestDto request, CancellationToken ct);
        Task<bool> ActivateUserAsync(string token, string email, CancellationToken ct);
        Task<bool> ResendActivationEmailAsync(string email, CancellationToken ct);
        Task<bool> IsEmailUsed(string email, CancellationToken ct);
        Task<bool> IsUsernameUsed(string username, CancellationToken ct);
    }
}
