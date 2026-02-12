using System.ComponentModel.DataAnnotations;

namespace Users.Application.Dtos.Requests
{
    public class UpdateProfileRequestDto
    {
        [EmailAddress]
        public string? NewEmail { get; set; }

        [Phone]
        public string? NewPhoneNumber { get; set; }

        [Required]
        public string CurrentPassword { get; set; } = null!;
    }
}
