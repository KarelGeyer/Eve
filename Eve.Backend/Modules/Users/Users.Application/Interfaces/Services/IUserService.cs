using System;
using System.Collections.Generic;
using System.Text;
using Common.Shared.Response;
using Domain.Entities;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.ResponseDtos;

namespace Users.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserBasicResponseDto> GetUserAsync(int id, CancellationToken ct);
        Task<UserFullResponseDto> GetUserFullAsync(int id, CancellationToken ct);
        Task<UserUpdateResponseDto> UpdateUserAsync(int id, UpdateProfileRequestDto request, CancellationToken ct);
        Task<bool> UpdateUserPasswordAsync(int id, ChangePasswordRequestDto request, CancellationToken ct);
        Task<bool> DeleteUserAsync(int id, string password, CancellationToken ct);
        Task<UserSettingsResponseDto?> GetUserSettings(int id, CancellationToken ct);
        Task<UserIdentityResponseDto?> GetUserIdentity(int id, CancellationToken ct);

        // Todo: Add more methods as needed
        // Task<UserGroupsResponseDto> GetUserActiveGroups(int id);
        // Task<UserRequestsResponseDto> GetUserActiveRequests(int id);
        // Task<UserRoleResponseDto> GetUserRoles(int id);
        // Task<UserUpdateNotifications> UpdateUserNotifications(UpdateNotificationsRequestDto request);
    }
}
