namespace Users.Application.Dtos.ResponseDtos
{
    public class UserBasicResponseDto
    {
        #region Base Properties
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        #endregion

        #region Settings
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime LastSignedIn { get; set; }
        #endregion
    }
}
