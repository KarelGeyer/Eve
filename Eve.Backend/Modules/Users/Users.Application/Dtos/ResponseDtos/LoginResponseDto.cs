using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Application.Dtos.ResponseDtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
