namespace Domain.Entities
{
    /// <summary>
    /// Represents a collection of settings and status information associated with a user account, including
    /// authentication, notification preferences, and account state.
    /// </summary>
    /// <remarks>UserSettings provides properties for tracking user activity, account status (such as blocked,
    /// deleted, or active), notification preferences, and authentication details. This class is typically used to
    /// manage and persist user-specific configuration and state within an application. Properties such as IsBlocked,
    /// IsActive, and IsDeleted indicate the current state of the user, while others like ActivationToken and
    /// ActivationTokenExpiration support account activation workflows. NotificationEnabled and EmailNotificationEnabled
    /// control whether the user receives application and email notifications, respectively.</remarks>
    public class UserSettings
    {
        public int UserId { get; set; }
        public string? LastAppVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the User is currently blocked from accessing the application.
        /// A blocked user is restricted from signing in or performing any actions until the block is lifted.
        /// </summary>
        public bool IsBlocked { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the current User is active.
        /// An active user is one who has completed the registration and activation
        /// process and has not been deactivated or blocked.
        /// </summary>
        public bool IsActive { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the User has been marked as deleted.
        /// User marked as deleted will be completely removed from the database after 30 days by background service.
        /// Until then, user will be marked as not-active and won't be able to log in.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the user's email address has been verified.
        /// </summary>
        public bool IsEmailVerified { get; set; } = false;
        public bool NotificationEnabled { get; set; } = true;
        public bool EmailNotificationEnabled { get; set; } = true;
        public bool NewsNotificationEnabled { get; set; } = true;
        public DateTime LastSignedIn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the block is scheduled to be lifted.
        /// </summary>
        public DateTime? UnblockDateTime { get; set; } = null;

        /// <summary>
        /// Gets or sets the number of login attempts made by the user.
        /// Max allowed attempts is 5. After that, user will be blocked for 60 minutes.
        /// </summary>
        public int LoginAttempts { get; set; } = 0;

        /// <summary>
        /// Gets or sets the activation token used to verify or activate a user account.
        /// </summary>
        public string? ActivationToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the activation token.
        /// </summary>
        /// <remarks>If the value is <see langword="null"/>, the activation token does not expire. Use
        /// this property to determine whether the token is still valid based on the current date and time.</remarks>
        public DateTime? ActivationTokenExpiration { get; set; }

        public string PrefferedLanguage { get; set; } = null!;
        public int Theme { get; set; }
        public string Timezone { get; set; } = null!;
        public bool TwoFactorEnabled { get; set; } = false;
        public string RecoveryEmail { get; set; } = null!;
        public int SecurityQuestion { get; set; } = 0;
        public string SecurityAnswer { get; set; } = null!;
        public bool MustChangePassword { get; set; } = false;

        public virtual User User { get; set; } = null!;
    }
}
