using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Application.Dtos.Requests
{
    public class RefreshRequestDto
    {
        public Guid DeviceUUID { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
