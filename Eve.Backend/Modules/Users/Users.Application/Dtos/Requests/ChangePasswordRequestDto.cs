using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Users.Application.Dtos.Requests
{
    public class ChangePasswordRequestDto
    {
        [Required]
        [MaxLength(32)]
        [MinLength(8)]
        public string NewPassword { get; set; } = null!;

        [Required]
        public string CurrentPassword { get; set; } = null!;
    }
}
