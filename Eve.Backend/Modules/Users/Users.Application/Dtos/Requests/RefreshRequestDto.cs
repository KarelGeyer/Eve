namespace Users.Application.Dtos.Requests
{
    public class RefreshRequestDto
    {
        public Guid DeviceUUID { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
