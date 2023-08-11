namespace JwtWebApi.Services.UserService
{
    public interface IUserService
    {
        string GetMyName();

        User CreateUser(User user);

        User GetUser(String userName);

        User SetRefreshToken(User user);

    }
}
