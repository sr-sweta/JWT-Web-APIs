using JwtWebApi.Data;
using System.Security.Claims;

namespace JwtWebApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtWebApiDbContext _jwtWebApiDbContext;

        public UserService(JwtWebApiDbContext jwtWebApiDbContext,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtWebApiDbContext = jwtWebApiDbContext;
        }

        string IUserService.GetMyName()
        {
            var result = string.Empty;
            if(_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return result;
        }

        User IUserService.CreateUser(User userData)
        {
            //if(userData == null)
            //{
                _jwtWebApiDbContext.Users.Add(userData);
                _jwtWebApiDbContext.SaveChanges();
            //}            

            return userData;
        }

        User IUserService.GetUser(String userName)
        {
            User gotUser = _jwtWebApiDbContext.Users.FirstOrDefault(i => i.UserName == userName);

            return gotUser;

        }

        User IUserService.SetRefreshToken(User user)
        {
            _jwtWebApiDbContext.Users.Update(user);
            _jwtWebApiDbContext.SaveChanges();

            return user;
        }
    }
}
