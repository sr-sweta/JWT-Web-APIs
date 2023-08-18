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
            Console.WriteLine("UserService userData OBJECT DATA\n------------------------------------------------------\n");
            Console.WriteLine("Username : " + userData.UserName);
            Console.WriteLine("PasswordHash : " + userData.PasswordHash);
            Console.WriteLine("PasswordSalt : " + userData.PasswordSalt);
            Console.WriteLine("DOB : " + userData.DOB);
            Console.WriteLine("PhNo : " + userData.PhNo);
            Console.WriteLine("Street : " + userData.Street);
            Console.WriteLine("City : " + userData.City);
            Console.WriteLine("Country : " + userData.Country);
            Console.WriteLine("State : " + userData.State);

            _jwtWebApiDbContext.Users.Add(userData);
            _jwtWebApiDbContext.SaveChanges();                      

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
