using TodoAuthApi.Models;

namespace TodoAuthApi.Services.UserService
{
    public interface IRegisterService
    {
        Task<User> RegisterAsync(User user, string password);
        Task<bool> UserExistsAsync(string userName, string email);
    }
}
