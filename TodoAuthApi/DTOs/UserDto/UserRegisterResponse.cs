namespace TodoAuthApi.DTOs.UserDto
{
    public class UserRegisterResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
    }
}
