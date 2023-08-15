using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtWebApi
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string DOB { get; set; } = string.Empty;

        public string PhNo { get; set; } = string.Empty;

        public string? Street { get; set; } = string.Empty;

        public string? City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
        
        public string RefreshToken { get; set; } =String.Empty;

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
        
    }
}
