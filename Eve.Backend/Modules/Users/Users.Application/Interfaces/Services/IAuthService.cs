using Common.Shared.Exceptions;
using Domain.Entities;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.ResponseDtos;

namespace Users.Application.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Asynchronously login a user through session.
        /// </summary>
        /// <param name="request">The login information to be used for login. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the registration operation.</param>
        /// <returns>a <see cref="LoginResponseDto"/> containing information about accessing the application and refreshing a session.</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ForbidenException"></exception>
        /// <exception cref="AccountLockedException"></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="ActionFailedException"></exception>
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct);

        /// <summary>
        /// Asynchronously logout a user session. Session is deleted if the operation is succsefull.
        /// </summary>
        /// <param name="deviceUuid">The session id to be deleted. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the registration operation.</param>
        /// <returns>a <see cref="true"/> if session was succesfully deleted.</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        Task<bool> LogoutAsync(Guid deviceUuid, CancellationToken ct);

        /// <summary>
        /// Asynchronously refresh a session for a <see cref="RefreshRequestDto.DeviceUUID"/>.
        /// </summary>
        /// <param name="request"><see cref="RefreshRequestDto"/>. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the registration operation.</param>
        /// <returns>a <see cref="LoginResponseDto"/> containing information about accessing the application and refreshing a session.</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="AccountLockedException"></exception>
        /// <exception cref="ActionFailedException"></exception>
        Task<LoginResponseDto> RefreshSessionAsync(RefreshRequestDto request, CancellationToken ct);

        /// <summary>
        /// Creates a new temporary password for a user and mark it as temporary through <see cref="UserSettings.MustChangePassword"/>.
        /// </summary>
        /// <param name="email">User's email. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the registration operation.</param>
        /// <returns>a <see cref="LoginResponseDto"/> containing information about accessing the application and refreshing a session.</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="AccountLockedException"></exception>
        /// <exception cref="ActionFailedException"></exception>
        Task<bool> RecoverPassword(string email, CancellationToken ct);
    }
}
