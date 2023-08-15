using System.ComponentModel.DataAnnotations;

namespace JwtWebApi
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Enter the Username.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter the Password.")]
        public string Password { get; set; } = string.Empty;
    }
}
