using Users.Domain.Entities;

namespace Users.Application.Dtos.ResponseDtos
{
    public class UserFullResponseDto
    {
        #region Base Properties
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        #endregion

        #region Settings
        public string LastAppVersion { get; set; } = null!;
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime LastSignedIn { get; set; }
        public DateTime? UnblockDateTime { get; set; }
        public int LoginAttempts { get; set; }
        #endregion

        #region Identity
        public string GoogleSubId { get; set; } = null!;
        public string AppleSubId { get; set; } = null!;
        public Guid LastLoginWith { get; set; }
        #endregion

        public List<UserSession> Sessions { get; set; } = new List<UserSession>();
    }
}
