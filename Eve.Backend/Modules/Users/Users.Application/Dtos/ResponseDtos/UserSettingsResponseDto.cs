using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Application.Dtos.ResponseDtos
{
    public class UserSettingsResponseDto
    {
        public string LastAppVersion { get; set; } = string.Empty;
        public string LastPlatform { get; set; } = string.Empty;
        public bool IsEmailNotificationsEnabled { get; set; }
        public bool IsSmsNotificationsEnabled { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime UnlockDateTime { get; set; }
        public DateTime LastSignedIn { get; set; }
        public int LoginAttempts { get; set; }
    }
}
