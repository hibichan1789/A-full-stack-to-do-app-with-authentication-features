using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoAuthApi.Context;
using TodoAuthApi.Models;

namespace TodoAuthApi.Services.UserService
{
    public class RegisterService : IRegisterService
    {
        private readonly MyContext _db;
        private readonly PasswordHasher<User> _passwordHasher;
        public RegisterService(MyContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            string passwordHash = _passwordHasher.HashPassword(user, password);
            user.PasswordHash = passwordHash;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            if(await UserExistsAsync(user.UserName, user.Email))
            {
                throw new InvalidOperationException("ユーザーは存在しています");
            }

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExistsAsync(string userName, string email)
        {
            return await _db.Users.AnyAsync(u => u.UserName == userName || u.Email == email);
        }
    }
}
