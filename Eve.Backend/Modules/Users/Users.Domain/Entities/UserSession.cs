namespace Users.Domain.Entities
{
    public class UserSession
    {
        /// <summary>
        /// UUID of a device that is bound to a session.
        /// </summary>
        public Guid SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Device { get; set; }
        public Guid DeviceId { get; set; }
        public string? DeviceToken { get; set; }
        public string? DeviceName { get; set; }
        public string? LastIpAddress { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public bool IsActive { get; set; }
    }
}
