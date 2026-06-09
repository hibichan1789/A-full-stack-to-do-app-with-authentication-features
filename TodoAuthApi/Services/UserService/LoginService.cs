using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoAuthApi.Context;
using TodoAuthApi.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TodoAuthApi.Services.UserService
{
    public class LoginService:ILoginService
    {
        private readonly MyContext _db;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config;
        public LoginService(MyContext db, IConfiguration config)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<User>();
            _config = config;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(user == null)
            {
                throw new InvalidOperationException("ログインに失敗しました");
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if(passwordResult == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("ログインに失敗しました");
            }




            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            // 秘密鍵を読み込み署名する準備をする
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userName", user.UserName)
            };

            var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
