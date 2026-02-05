using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Application.Dtos.ResponseDtos
{
    public class UserUpdateResponseDto
    {
        public string NewEmail { get; set; }
        public string NewPhoneNumber { get; set; }
    }
}
