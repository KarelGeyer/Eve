using System;
using System.Collections.Generic;
using System.Text;
using Users.Domain.Enums;

namespace Users.Application.Dtos.Requests
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid DeviceUUID { get; set; }
        public string? DeviceName { get; set; }
        public DeviceType Platform { get; set; }
    }
}
