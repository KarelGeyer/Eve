using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Domain.Entities;
using Users.Application.Dtos.ResponseDtos;

namespace Users.Application.Extensions
{
    /// <summary>
    /// Extension methods for converting User entities to various Data Transfer Objects (DTOs) used in the application layer.
    /// </summary>
    public static class UserDtoExtensions
    {
        public static UserBasicResponseDto ToBasicDto(this User user)
        {
            return new UserBasicResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                DateOfBirth = user.DateOfBirth,

                IsBlocked = user.Settings!.IsBlocked,
                IsActive = user.Settings!.IsActive,
                IsEmailVerified = user.Settings!.IsEmailVerified,
                LastSignedIn = user.Settings.LastSignedIn,
            };
        }

        public static UserFullResponseDto ToFullDto(this User user, string subId)
        {
            return new UserFullResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                DateOfBirth = user.DateOfBirth,

                IsBlocked = user.Settings!.IsBlocked,
                IsActive = user.Settings!.IsActive,
                IsEmailVerified = user.Settings!.IsEmailVerified,
                LastSignedIn = user.Settings!.LastSignedIn,
                UbblockDateTime = user.Settings!.UnblockDateTime,
                LoginAttempts = user.Settings!.LoginAttempts,

                SubId = subId,
                RefreshToken = user.Identity!.RefreshToken,
                DeviceToken = user.Identity!.DeviceToken,
                DeviceType = user.Identity!.DeviceType,
            };
        }

        public static UserIdentityResponseDto ToIdentityDto(this UserIdentity identity, string subId)
        {
            return new UserIdentityResponseDto()
            {
                SubId = subId,
                RefreshToken = identity!.RefreshToken,
                DeviceToken = identity!.DeviceToken,
                DeviceType = identity!.DeviceType,
            };
        }

        public static UserSettingsResponseDto ToSettingsDto(this UserSettings settings)
        {
            return new UserSettingsResponseDto()
            {
                LastAppVersion = settings!.LastAppVersion,
                LastPlatform = settings!.LastPlatform,
                IsEmailNotificationsEnabled = settings!.EmailNotificationEnabled,
                IsSmsNotificationsEnabled = settings!.NotificationEnabled,
                IsBlocked = settings!.IsBlocked,
                IsActive = settings!.IsActive,
                IsEmailVerified = settings!.IsEmailVerified,
                UnlockDateTime = settings!.UnblockDateTime,
                LastSignedIn = settings!.LastSignedIn,
                LoginAttempts = settings!.LoginAttempts,
            };
        }
    }
}
