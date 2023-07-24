using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        // The IConfiguration interface need to be injected as dependency in the Controller and then later used throughout the Controller.
        // The IConfiguration interface is used to read Settings and Connection Strings from AppSettings.json file.
        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet, Authorize]

        // This method return userName of the user. This has been accessed using UserService class.
        // HttpContext has been used to get the access to the response of the user.
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();

            return Ok(userName);
        }


        [HttpPost("register")]
        // Salting is done to save the password as for same password to different form while storing in database
        // so that it could be protected from hacking of accounts having same actual password.
        // READ SALTING METHOD IN CRYTPOGRAPHY TO UNDERSTAND IT MORE.......
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
            if (request.Username != user.UserName)
            {
                return BadRequest("Username not valid.");
            }

            if (!VerifyPasswordHash(request.Password))
            {
                return BadRequest("Wrong Password.");
            }

            string token = CreateToken(user);

            RefreshToken refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]

        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token!");
            }

            else if(user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken(); ;
            SetRefreshToken(newRefreshToken);

            return Ok(token);

        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7)
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),

                // Below claim is to authorize admin to access the other information using JWT TOKEN
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
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

