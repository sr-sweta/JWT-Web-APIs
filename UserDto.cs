using Microsoft.OData.Edm;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace JwtWebApi
{
    public class UserDto
    {

        [Required(ErrorMessage = "Enter the Username.")]
        public string Username { get; set; } = string.Empty ;

        [Required(ErrorMessage = "Enter the Password.")]
        public string Password { get; set; } = string.Empty ;

        [Required(ErrorMessage = "Enter the DOB.")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string PhNo { get; set; } = string.Empty;

        public string? Street { get; set; } = string.Empty;

        public string? City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
    }
}
