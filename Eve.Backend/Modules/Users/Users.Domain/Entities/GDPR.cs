using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    /// <summary>
    /// Represents a user's acceptance of GDPR-related terms, including Terms of Service and Privacy Policy, along with
    /// associated metadata.
    /// </summary>
    /// <remarks>This class is typically used to track compliance with legal requirements regarding user
    /// consent. It associates acceptance timestamps and version information with a specific user. Use this type to
    /// audit or enforce GDPR-related policies within an application.</remarks>
    public class GDPR
    {
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the user accepted the terms of service.
        /// </summary>
        public DateTime TosAcceptAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the user accepted the privacy policy.
        /// </summary>
        public DateTime PrivacyPolicyAcceptedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the version identifier of the terms of service associated with the current context.
        /// </summary>
        public string? TosVersion { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
