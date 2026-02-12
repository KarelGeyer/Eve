namespace Users.Application.Dtos.ResponseDtos
{
    public class UserSettingsResponseDto
    {
        public string? LastAppVersion { get; set; }
        public bool AreNotificationsEnabled { get; set; }
        public bool IsEmailNotificationsEnabled { get; set; }
        public bool IsNewsNotificationsEnabled { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime? UnlockDateTime { get; set; }
        public DateTime LastSignedIn { get; set; }
        public int LoginAttempts { get; set; }
        public string? PrefferedLanguage { get; set; }
        public int Theme { get; set; }
        public string? TimeZone { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int SecurityQuestion { get; set; }
        public string? SecurityAnswer { get; set; }
        public bool MustChangePassword { get; set; }
    }
}
