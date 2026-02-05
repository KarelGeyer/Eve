using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Application.Dtos.ResponseDtos
{
    public class UserIdentityResponseDto
    {
        public string? SubId { get; set; }
        public string? RefreshToken { get; set; }
        public string? DeviceToken { get; set; }
        public string? DeviceType { get; set; }
    }
}
