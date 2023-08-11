using JwtWebApi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApi.Data
{
    public class JwtWebApiDbContext : DbContext
    {
        public JwtWebApiDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
