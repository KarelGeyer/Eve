namespace Users.Application.Dtos.ResponseDtos
{
    public class UserRegistrationResponseDto
    {
        public string EmailCheckResult { get; set; } = string.Empty;
        public string IsSuccesfull { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
