namespace Users.Application.Dtos.Requests
{
    public class UserRegistrationRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public bool AgreedToTos { get; set; }
        public bool AgreedToGdpr { get; set; }
        public string PrefferedLanguage { get; set; } = string.Empty;
        public int SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; } = string.Empty;
    }
}
