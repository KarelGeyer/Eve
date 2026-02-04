using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entites
{
    public class UserSettings
    {
        public int UserId { get; set; }
        public string LastAppVersion { get; set; } = string.Empty;
        public string LastPlatform { get; set; } = string.Empty;
        public bool IsBlocked { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = false;
        public DateTime LastSignedIn { get; set; } = DateTime.UtcNow;
        public DateTime UnblockDateTime { get; set; } = DateTime.UtcNow;
        public int LoginAttempts { get; set; } = 0;
    }
}
