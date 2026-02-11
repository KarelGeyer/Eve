using System;
using System.Collections.Generic;
using System.Text;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.ResponseDtos;

namespace Users.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct);
        Task<bool> LogoutAsync(Guid deviceUuid, CancellationToken ct);
        Task<LoginResponseDto> RefreshSessionAsync(RefreshRequestDto request, CancellationToken ct);
        Task<bool> RecoverPassword(string email, CancellationToken ct);
    }
}
