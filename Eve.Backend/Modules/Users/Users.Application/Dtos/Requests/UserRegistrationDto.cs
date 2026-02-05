using System;
using System.Collections.Generic;
using System.Text;
using Users.Domain.Enums;

namespace Users.Application.Dtos.Requests
{
    public class UserRegistrationRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DeviceType DeviceType { get; set; }
        public bool AgreedToTos { get; set; }
        public bool AgreedToGdpr { get; set; }
    }
}
