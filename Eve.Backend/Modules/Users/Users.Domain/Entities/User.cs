using Users.Domain.Entities;

namespace Domain.Entities
{
    /// <summary>
    /// Represents an application user, including identification, contact information, and related settings.
    /// </summary>
    /// <remarks>The User class provides core user profile data and links to associated settings and identity
    /// information. Use this type to access or modify user details within the application. Related properties such as
    /// Settings, Identity, and GDPR contain additional user-specific configuration and compliance
    /// information.</remarks>
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PasswordHash { get; set; } = null!;

        public virtual UserSettings? Settings { get; set; } = null;
        public virtual UserIdentity? Identity { get; set; } = null;
        public virtual GDPR? GDPR { get; set; } = null;
        public virtual ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
    }
}
