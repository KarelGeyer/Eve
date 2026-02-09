using System;
using System.Collections.Generic;
using System.Text;

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

        /// <summary>
        /// Gets or sets the Apple subscription identifier associated with the user or transaction.
        /// </summary>
        public string AppleSubId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the refresh token used to obtain a new access token when the current one expires.
        /// </summary>
        public string RefreshToken { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique token used to identify the device for authentication or push notification purposes.
        /// </summary>
        public string DeviceToken { get; set; } = null!;
        public string DeviceType { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
