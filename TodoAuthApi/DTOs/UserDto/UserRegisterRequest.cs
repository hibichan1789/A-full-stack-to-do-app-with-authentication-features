using System.ComponentModel.DataAnnotations;

namespace TodoAuthApi.DTOs.UserDto
{
    public class UserRegisterRequest
    {
        [Required]
        public string UserName { get; set; } = String.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;
        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
