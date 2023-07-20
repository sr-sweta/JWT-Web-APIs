using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace JwtWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();

        [HttpPost("register")]

        public async Task<ActionResult<User>> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out Byte[] passwordSalt);

            user.UserName = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Ok(user);

        }


        [HttpPost("Login")]

        public async Task<ActionResult<string>> Login(UserDto request)
        {
            if(request.Username != user.UserName)
            {
                return BadRequest("Username not valid.");
            }

            if (!VerifyPasswordHash(request.Password))
            {
                return BadRequest("Wrong Password.");
            }

            return Ok("Crazy token works!!");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedPasswordHash.SequenceEqual(user.PasswordHash);
            }
        }
    }
}
