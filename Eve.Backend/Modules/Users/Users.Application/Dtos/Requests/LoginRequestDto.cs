using System.ComponentModel.DataAnnotations;
using Users.Domain.Enums;

namespace Users.Application.Dtos.Requests
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        public Guid DeviceUUID { get; set; }
        public string? DeviceName { get; set; }
        public DeviceType Platform { get; set; }
    }
}
