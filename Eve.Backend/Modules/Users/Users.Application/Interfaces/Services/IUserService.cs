using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.ResponseDtos;

namespace Users.Application.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a user by their unique identifier and returns basic information about the user.
        /// This method is typically used for displaying user profiles or for administrative purposes where only essential user details are required.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns>A <see cref="User"/> without additional informations</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        Task<UserBasicResponseDto> GetUserAsync(int id, CancellationToken ct);

        /// <summary>
        /// Retrieves a user by their unique identifier and returns comprehensive information about the user,
        /// including all relevant details such as contact information, account status, and other profile-related data.
        /// This method is typically used for administrative purposes or when detailed user information is required for specific operations.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns>A <see cref="User"/> with all additional informations</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        Task<UserFullResponseDto> GetUserFullAsync(int id, CancellationToken ct);

        /// <summary>
        /// Attempts to update a user's profile information, such as email and phone number, based on the provided update request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns>A <see cref="UserUpdateResponseDto"/></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ActionFailedException"></exception>
        Task<UserUpdateResponseDto?> UpdateUserAsync(int id, UpdateProfileRequestDto request, CancellationToken ct);

        /// <summary>
        /// Attempts to update a user's password based on the provided change password request, which includes the current password and the new desired password.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns>True if password was updated</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ActionFailedException"></exception>
        Task<bool> UpdateUserPasswordAsync(int id, ChangePasswordRequestDto request, CancellationToken ct);

        /// <summary>
        /// Attempts to mark a user's account for deletion based on the provided password for verification.
        /// The user will be scheduled for deletion and will be unable to log in during the waiting period.
        /// This method ensures that only users who can provide their current password can initiate account deletion,
        /// adding a layer of security to prevent unauthorized deletions.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="ct"></param>
        /// <returns>True if account was marked as deleted and scheduled for Hard Delete</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        Task<bool> DeleteUserAsync(int id, string password, CancellationToken ct);

        /// <summary>
        /// Retrieves the settings and preferences associated with a user, such as notification preferences, account status, and other relevant configurations.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns>A <see cref="UserSettingsResponseDto"/></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        Task<UserSettingsResponseDto> GetUserSettings(int id, CancellationToken ct);

        /// <summary>
        /// Retrieves the identity information associated with a user, such as authentication details, linked accounts, and other relevant identity-related data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns>A <see cref="UserIdentityResponseDto"/></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        Task<UserIdentityResponseDto> GetUserIdentity(int id, CancellationToken ct);

        // Todo: Add more methods as needed
        // Task<UserGroupsResponseDto> GetUserActiveGroups(int id);
        // Task<UserRequestsResponseDto> GetUserActiveRequests(int id);
        // Task<UserRoleResponseDto> GetUserRoles(int id);
        // Task<UserUpdateNotifications> UpdateUserNotifications(UpdateNotificationsRequestDto request);
    }
}
