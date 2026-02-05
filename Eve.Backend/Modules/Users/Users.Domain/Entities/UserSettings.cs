using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class UserSettings
    {
        public int UserId { get; set; }
        public string? LastAppVersion { get; set; }
        public string? LastPlatform { get; set; }
        public bool IsBlocked { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public bool NotificationEnabled { get; set; } = true;
        public bool EmailNotificationEnabled { get; set; } = true;
        public DateTime LastSignedIn { get; set; } = DateTime.UtcNow;
        public DateTime UnblockDateTime { get; set; } = DateTime.UtcNow;
        public int LoginAttempts { get; set; } = 0;

        public virtual User User { get; set; } = null!;
    }
}
