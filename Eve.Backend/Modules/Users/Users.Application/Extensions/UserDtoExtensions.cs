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

        public static UserFullResponseDto ToFullDto(this User user)
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
                UnblockDateTime = user.Settings!.UnblockDateTime,
                LoginAttempts = user.Settings!.LoginAttempts,

                GoogleSubId = user.Identity!.GoogleSubId,
                AppleSubId = user.Identity!.AppleSubId,

                Sessions = user.Sessions.ToList(),
            };
        }

        public static UserIdentityResponseDto ToIdentityDto(this UserIdentity identity, string subId)
        {
            return new UserIdentityResponseDto()
            {
                GoogleSubId = identity!.GoogleSubId,
                AppleSubId = identity!.AppleSubId,
                LastLoginWith = identity!.LastLoginWith,
            };
        }

        public static UserSettingsResponseDto ToSettingsDto(this UserSettings settings)
        {
            return new UserSettingsResponseDto()
            {
                LastAppVersion = settings!.LastAppVersion,
                AreNotificationsEnabled = settings!.NotificationEnabled,
                IsEmailNotificationsEnabled = settings!.EmailNotificationEnabled,
                IsNewsNotificationsEnabled = settings!.NotificationEnabled,
                IsBlocked = settings!.IsBlocked,
                IsActive = settings!.IsActive,
                IsEmailVerified = settings!.IsEmailVerified,
                UnlockDateTime = settings!.UnblockDateTime,
                LastSignedIn = settings!.LastSignedIn,
                LoginAttempts = settings!.LoginAttempts,
                PrefferedLanguage = settings!.PrefferedLanguage,
                Theme = settings!.Theme,
                TimeZone = settings!.Timezone,
                TwoFactorEnabled = settings!.TwoFactorEnabled,
                SecurityQuestion = settings!.SecurityQuestion,
                SecurityAnswer = settings!.SecurityAnswer,
                MustChangePassword = settings!.MustChangePassword,
            };
        }
    }
}
