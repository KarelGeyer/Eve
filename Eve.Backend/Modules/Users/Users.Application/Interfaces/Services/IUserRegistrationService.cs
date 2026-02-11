using System;
using System.Collections.Generic;
using System.Text;
using Common.Shared.Exceptions;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.ResponseDtos;

namespace Users.Application.Interfaces
{
    public interface IUserRegistrationService
    {
        /// <summary>
        /// Asynchronously registers a new user with the provided registration details.
        /// </summary>
        /// <param name="request">The user registration information to be used for creating the new user account. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the registration operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user was
        /// successfully registered; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="EntityAlreadyExistsException"></exception>
        /// <exception cref="ActionFailedException"></exception>
        Task<bool> RegisterUserAsync(UserRegistrationRequestDto request, CancellationToken ct);

        /// <summary>
        /// Activates a user account asynchronously using the specified activation token.
        /// </summary>
        /// <param name="token">The activation token associated with the user account to be activated. Cannot be null or empty.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the activation operation.</param>
        /// <returns> Returns 0 if the token is invalid or no account was activated and 1 if account was successfuly activated</returns>
        /// <exception cref="SecurityException"></exception>
        Task<int> ActivateUserAsync(string token, CancellationToken ct);

        /// <summary>
        /// Attempts to resend the activation email to the specified user asynchronously.
        /// </summary>
        /// <remarks>If the specified email address is not associated with an existing user or the user is
        /// already activated, the activation email will not be resent</remarks>
        /// <param name="email">The email address of the user to whom the activation email will be sent. Cannot be null or empty.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The task result is <see langword="true"/> if the
        /// activation email was successfully resent.</returns>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        Task<bool> ResendActivationEmailAsync(string email, CancellationToken ct);

        /// <summary>
        /// Finds out if the email address is already associated with an existing user account, which would prevent registration with that email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns>True if email is already being used by another account</returns>
        Task<bool> IsEmailUsed(string email, CancellationToken ct);

        /// <summary>
        /// Finds out if the username is already associated with an existing user account, which would prevent registration with that username.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ct"></param>
        /// <returns>True is username is already being used by another account</returns>
        Task<bool> IsUsernameUsed(string username, CancellationToken ct);

        /// <summary>
        /// Checks if the provided email address is on the list of banned email addresses, which would prevent registration with that email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns>True if email is valid and not on a blacklist</returns>
        Task<bool> IsEmailAdressBanned(string email, CancellationToken ct);
    }
}
