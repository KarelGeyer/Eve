namespace Domain.Entities
{
    /// <summary>
    /// Represents a user's authentication and device identity information, including platform-specific identifiers and
    /// tokens.
    /// </summary>
    /// <remarks>The UserIdentity class is typically used to associate authentication credentials and device
    /// details with a user account. It supports multiple authentication providers, such as Google and Apple, and stores
    /// relevant tokens for session management and device tracking. This class is commonly used in scenarios where user
    /// authentication, device registration, or token management is required.</remarks>
    public class UserIdentity
    {
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier associated with the Google account subscription.
        /// </summary>
        public string GoogleSubId { get; set; } = null!;
        public string GoogleSubEmail { get; set; } = null!;
        public DateTime? GoogleSubCreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the Apple subscription identifier associated with the user or transaction.
        /// </summary>
        public string AppleSubId { get; set; } = null!;
        public string AppleSubEmail { get; set; } = null!;
        public DateTime? AppleSubCreatedAt { get; set; }

        /// <summary>
        /// Reference to a device identifier, relatedTo <see cref="UserSession"/>
        /// </summary>
        public Guid LastLoginWith { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
