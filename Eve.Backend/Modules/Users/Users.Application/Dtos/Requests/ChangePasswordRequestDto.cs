using System.ComponentModel.DataAnnotations;

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
