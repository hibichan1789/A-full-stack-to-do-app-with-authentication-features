using System.ComponentModel.DataAnnotations;

namespace TodoAuthApi.DTOs.UserDto
{
    public class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
