namespace Users.Application.Dtos.ResponseDtos
{
    public class UserIdentityResponseDto
    {
        public string? GoogleSubId { get; set; }
        public string? AppleSubId { get; set; }
        public Guid? LastLoginWith { get; set; }
    }
}
