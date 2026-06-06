using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TodoAuthApi.DTOs.UserDto;
using TodoAuthApi.Models;
using TodoAuthApi.Services.UserService;

namespace TodoAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        public UserController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterRequest userRegisterRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool alreadyExits = await _registerService.UserExistsAsync(userRegisterRequest.UserName, userRegisterRequest.Email);
            if (alreadyExits)
            {
                return BadRequest(new { message = "ユーザーは存在しています" });
            }

            var userBeforeRegistered = new User
            {
                UserName = userRegisterRequest.UserName,
                Email = userRegisterRequest.Email
            };

            User userAfterRegistered;
            try
            {
                userAfterRegistered = await _registerService.RegisterAsync(userBeforeRegistered, userRegisterRequest.Password);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Problem("サーバエラーです");
            }

            // ここでresponse用のDTOを作っておくほうが無難かも
            var userRegisterResponse = new UserRegisterResponse
            {
                Id = userAfterRegistered.Id,
                UserName = userAfterRegistered.UserName,
                Email = userAfterRegistered.Email
            };
            return Ok(userRegisterResponse);
        }
    }
}
